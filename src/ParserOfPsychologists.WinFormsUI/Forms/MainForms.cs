namespace ParserOfPsychologists.WinFormsUI;

public partial class MainForms : Form
{
    private readonly string _fromLineStartsWith = "с";
    private readonly string _toLineStartsWith = "по";

    private readonly ICityHandlerModule _stateOfCity;
    private readonly IParserWebRequestsFacade _facade;

    public MainForms(IParserWebRequestsFacade facade, ICityHandlerModule stateOfCityModule)
    {
        _facade = facade;
        _stateOfCity = stateOfCityModule;

        _stateOfCity.CityChanged += OnCityChanged;

        InitializeComponent();
        RegisterFormEvents();
    }

    private void RegisterFormEvents()
    {
        ActionOnEventsToLoadAndCloseForm();
        ActionOnEventsChangeParserSettings();
    }

    private void ActionOnEventsToLoadAndCloseForm()
    {
        var cities = async () => (await _facade.FindCityAsync(string.Empty)).ToArray();
        this.Load += async (s, e) => this.citiesBox.Items.AddRange(await cities.Invoke());
    }

    private void ActionOnEventsChangeParserSettings()
    {
        this.citiesBox.SelectedValueChanged += async (s, e) =>
        {
            if (s is ComboBox box && _stateOfCity.IsChanged(box.Text))
                await _stateOfCity.ChangeCityAsync(box.Text);
        };

        this.parsePageFromBox.SelectedValueChanged += (s, e) =>
        {
            if (s is ComboBox fromBox && ExtractIntOf(fromBox.Text) is int from)
            {
                var textTo = this.parsePageToBox.Text;

                this.parsePageToBox.Items.Clear();
                this.parsePageToBox.Items.AddRange(CreateSequenceOfPages(_toLineStartsWith, from, _stateOfCity.NumberOfPagesAvailable - from + 1));

                this.parsePageToBox.Text = from < ExtractIntOf(textTo) ? textTo : $"{_toLineStartsWith} {from}";
            }
        };
    }

    private void OnCityChanged(object? obj, StateOfCityEventArgs args)
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

    private static int ExtractIntOf(string text) => Convert.ToInt32(text.Split(' ').LastOrDefault());

    private static string[] CreateSequenceOfPages(string lineStartsWith, int pageNumberingStartsWith, int pagesAvailable)
    {
        if (!lineStartsWith.All(x => char.IsLetter(x)))
            throw new ArgumentException("String should consist of the letter.", nameof(lineStartsWith));

        if (pageNumberingStartsWith == 0) return Array.Empty<string>();

        return Enumerable.Range(pageNumberingStartsWith, pagesAvailable).Select(x => $"{lineStartsWith} {x}").ToArray();
    }
}