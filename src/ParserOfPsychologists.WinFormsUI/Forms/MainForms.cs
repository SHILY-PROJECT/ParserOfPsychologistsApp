namespace ParserOfPsychologists.WinFormsUI;

public partial class MainForms : Form
{
    private readonly string _fromLineStartsWith = "с";
    private readonly string _toLineStartsWith = "по";

    private readonly IApplicationFacade _facade;
    private readonly IParserSettings _parserSettings;

    public MainForms(IApplicationFacade facade, IParserSettings parserSettings)
    {
        _facade = facade;
        _parserSettings = parserSettings;

        InitializeComponent();
        RegisterFormEvents();
    }

    private void RegisterFormEvents()
    {
        ActionOnEventsToLoadAndCloseForm();
        ActionOnEventsParser();        
    }

    private void ActionOnEventsToLoadAndCloseForm()
    {
        _facade.CityHandler.CityChanged += OnCityChanged;

        this.Load += async (s, e) =>
        {
            var cities = async () => (await _facade.FindCityAsync(string.Empty)).ToArray();
            this.citiesBox.Items.AddRange(await cities.Invoke());
        };
    }

    private void ActionOnEventsParser()
    {
        this.citiesBox.SelectedValueChanged += async (s, e) =>
        {
            if (s is ComboBox box && _facade.CityHandler.IsChanged(_parserSettings.CityOnInput = box.Text))
            {
                await _facade.ChangeCityAsync();
            }
        };

        this.parsePageFromBox.SelectedValueChanged += (s, e) =>
        {
            if (s is ComboBox fromBox && PageNumberOf(fromBox.Text) is int from)
            {
                var textTo = this.parsePageToBox.Text;

                this.parsePageToBox.Items.Clear();
                this.parsePageToBox.Items.AddRange(CreateSequenceOfPages(_toLineStartsWith, from, _facade.CityHandler.NumberOfAvailablePagesForCurrentCity - from + 1));

                this.parsePageToBox.Text = from < PageNumberOf(textTo) ? textTo : $"{_toLineStartsWith} {from}";
            }
        };

        this.startParsing.Click += async (s, e) =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.citiesBox.Text))
                    throw new InvalidOperationException("Select cities from the list.");

                _parserSettings.SetTimeouts(this.timeoutsBox.Text);
                _parserSettings.ToParsePagesTo = PageNumberOf(this.parsePageToBox.Text);
                _parserSettings.ToParsePagesFrom = PageNumberOf(this.parsePageFromBox.Text);

                await Parse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ooops...", MessageBoxButtons.OK);
            }
        };
    }

    private async Task Parse()
    {
        var users = await _facade.ParseUsersByCityAsync();

    }

    private void OnCityChanged(object? obj, CityHandlerModuleEventArgs args)
    {
        this.parsePageFromBox.Items.Clear();
        this.parsePageToBox.Items.Clear();

        if (CreateSequenceOfPages(_fromLineStartsWith, 1, args.PagesAvailable) is string[] arrFrom && arrFrom.Any() &&
            CreateSequenceOfPages(_toLineStartsWith, 1, args.PagesAvailable) is string[] arrTo && arrTo.Any())
        {
            this.parsePageFromBox.Items.AddRange(arrFrom);
            this.parsePageToBox.Items.AddRange(arrTo);

            this.parsePageToBox.Text = this.parsePageToBox.Items[this.parsePageToBox.Items.Count - 1] as string;
            this.parsePageFromBox.Text = this.parsePageFromBox.Items[0] as string;
        }
    }

    private static int PageNumberOf(string pageNumberWithPrefix)
    {
        if (int.TryParse(pageNumberWithPrefix.Split(' ').LastOrDefault(), out var value)) return value;
        throw new ArgumentException("Failed to retrieve page number.", nameof(pageNumberWithPrefix));
    }

    private static string[] CreateSequenceOfPages(string lineStartsWith, int pageNumberingStartsWith, int pagesAvailable)
    {
        if (!lineStartsWith.All(x => char.IsLetter(x)))
            throw new ArgumentException("String should consist of the letter.", nameof(lineStartsWith));

        if (pageNumberingStartsWith == 0) return Array.Empty<string>();

        return Enumerable.Range(pageNumberingStartsWith, pagesAvailable).Select(x => $"{lineStartsWith} {x}").ToArray();
    }
}