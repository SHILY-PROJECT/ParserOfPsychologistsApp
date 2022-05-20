namespace ParserOfPsychologists.WinFormsUI;

public partial class MainForms : Form
{
    private readonly string _fromLineStartsWith = "с";
    private readonly string _toLineStartsWith = "по";

    private IDictionary<string, string> _defaultCities = new Dictionary<string, string>();

    private readonly IServiceProvider _services;
    private readonly IApplicationFacade _facade;
    private readonly IParserSettings _parserSettings;

    public MainForms(IServiceProvider services, IApplicationFacade facade, IParserSettings parserSettings)
    {
        _services = services;
        _facade = facade;
        _parserSettings = parserSettings;

        InitializeComponent();
        RegisterFormEvents();
    }

    private void RegisterFormEvents()
    {
        this.ActionOnEventsToLoadAndCloseForm();
        this.ActionOnEventsParser();
    }

    private void ActionOnEventsToLoadAndCloseForm()
    {
        _facade.ApplicationInfoSender += this.OnShowMessageBox;
        _facade.CityHandler.CityChanged += this.OnChangedCityField;

        this.Load += (s, e) => this.OnChangeTitle();
        this.Load += (s, e) => this.ChangeParserControlEnabled(false);
        this.Load += (s, e) => this.ChangeAuthControlEnabled(false);
        this.Load += async (s, e) => this.cityBox.Items.AddRange((_defaultCities = await _facade.GetDefaultCities()).Keys.ToArray());      
    }

    private void ActionOnEventsParser()
    {
        this.clearToCityBoxButton.Click += (s, e) => this.OnClickClearCityField();

        this.cityBox.Click += (s, e) => OnClockCityBox(s, e);
        this.cityBox.SelectedValueChanged += async (s, e) => await this.OnValueChangedCitiesBox(s, e);
        this.cityBox.TextUpdate += async (s, e) => await this.OnEntryInCityField(s, e);

        this.parsePageFromBox.SelectedValueChanged += (s, e) => this.OnPageFromValueChanged(s, e);
        this.startParsingButton.Click += async (s, e) => await this.OnParseUsersByCityAsync();
        this.openResultsButton.Click += (s, e) => _facade.OpenResultsFolder();

        this.connectAccount.CheckedChanged += async (s, e) => await OnConnectAccount(s, e);
    }

    private void OnClickClearCityField()
    {
        this.ChangeParserControlEnabled(false);
        this.cityBox.Text = string.Empty;
        this.parsePageToBox.Items.Clear();
        this.parsePageFromBox.Items.Clear();
        this._facade.CityHandler.ResetCurrentCity();
        this.cityBox.Items.Clear();
        this.cityBox.Items.AddRange(_defaultCities.Keys.ToArray());
    }

    private void OnPageFromValueChanged(object? source, EventArgs args)
    {
        if (source is not ComboBox from) return;

        var value = this.PageNumberOf(from.Text);
        var textTo = this.parsePageToBox.Text;

        this.parsePageToBox.Items.Clear();
        this.parsePageToBox.Items.AddRange(
            CreateSequenceOfPages(_toLineStartsWith, value, _facade.CityHandler.NumberOfAvailablePagesForCurrentCity - value + 1));

        this.parsePageToBox.Text = value < this.PageNumberOf(textTo) ? textTo : $"{_toLineStartsWith} {value}";
    }

    private void OnClockCityBox(object? source, EventArgs args)
    {
        if (source is not ComboBox box) return;
        else box.DroppedDown = true;
    }

    private async Task OnConnectAccount(object? source, EventArgs args)
    {
        if (source is not CheckBox box) return;

        if (box.Checked)
        {
            this.ChangeAuthControlEnabled(true);
            this.captchaBox.Image = (await ((AuthorizationModule)_facade.Authorization).UpdateCaptcha()).Img;
        }
        else
        {
            this.ChangeAuthControlEnabled(false);
            this.captchaBox.Image = default;
        }
    }

    private async Task OnValueChangedCitiesBox(object? source, EventArgs args)
    {
        if (source is not ComboBox box || !_facade.CityHandler.IsChanged(box.Text)) return;
        else _parserSettings.CityOnInput = box.Text;

        this.ChangeParserControlEnabled(false);
        await _facade.ChangeCityAsync();
    }

    private async Task OnEntryInCityField(object? source, EventArgs args)
    {
        if (source is not ComboBox box) return;

        if (!string.IsNullOrWhiteSpace(box.Text) && !box.Items.Contains(box.Text))
        {
            var foundCities = (await _facade.FindCityAsync(box.Text)).Keys.ToArray();

            box.Items.Clear();

            box.Focus();
            box.SelectionStart = box.Text.Length;

            if (foundCities.Any())
            {
                box.Items.AddRange(foundCities);
                box.DroppedDown = true;              
            }
            return;
        }

        if (box.Text.StartsWith(" ")) box.Text = box.Text.Remove(0, 1);
        
        if (!Enumerable.SequenceEqual(_defaultCities.Keys.Cast<string>(), box.Items.Cast<string>()))
        {
            box.Items.Clear();

            box.Focus();
            box.SelectionStart = box.Text.Length;

            box.Items.AddRange(_defaultCities.Keys.ToArray());
            box.DroppedDown = true;
        }
    }

    private async Task OnParseUsersByCityAsync()
    {
        using var waitForm = _services.GetRequiredService<WaitForm>();

        try
        {
            if (string.IsNullOrWhiteSpace(this.cityBox.Text))
                throw new InvalidOperationException("Select cities from the list.");

            _parserSettings.SetTimeouts(this.timeoutsBox.Text);
            _parserSettings.PageTo = this.PageNumberOf(this.parsePageToBox.Text);
            _parserSettings.PageFrom = this.PageNumberOf(this.parsePageFromBox.Text);

            waitForm.Owner = this;
            waitForm.ShowInTaskbar = false;
            this.Enabled = false;

            waitForm.Show();
            await _facade.ParseUsersByCityAsync();
        }
        catch (Exception ex)
        {
            this.OnShowMessageBox(this, new(ex.Message));
        }
        finally
        {
            this.Enabled = true;
        }
    }

    private void ChangeParserControlEnabled(bool enableOrDisable)
    {
        this.startParsingButton.Enabled = enableOrDisable;
        this.parsePageFromBox.Enabled = enableOrDisable;
        this.parsePageToBox.Enabled = enableOrDisable;
    }

    private void ChangeAuthControlEnabled(bool enableOrDisable)
    {
        this.loginInput.Enabled = enableOrDisable;
        this.passInput.Enabled = enableOrDisable;
        this.captchaInput.Enabled = enableOrDisable;
        this.captchaBox.Enabled = enableOrDisable;
        this.authButton.Enabled = enableOrDisable;
    }

    private void OnChangedCityField(object? source, CityHandlerModuleEventArgs args)
    {
        this.parsePageFromBox.Items.Clear();
        this.parsePageToBox.Items.Clear();

        if (this.CreateSequenceOfPages(_fromLineStartsWith, 1, args.PagesAvailable) is string[] arrFrom && arrFrom.Any() &&
            this.CreateSequenceOfPages(_toLineStartsWith, 1, args.PagesAvailable) is string[] arrTo && arrTo.Any())
        {
            this.parsePageFromBox.Items.AddRange(arrFrom);
            this.parsePageToBox.Items.AddRange(arrTo);

            this.parsePageToBox.Text = this.parsePageToBox.Items[this.parsePageToBox.Items.Count - 1] as string;
            this.parsePageFromBox.Text = this.parsePageFromBox.Items[0] as string;

            this.ChangeParserControlEnabled(true);
        }
    }

    private int PageNumberOf(string pageNumberWithPrefix)
    {
        if (int.TryParse(pageNumberWithPrefix.Split(' ').LastOrDefault(), out var value)) return value;
        throw new ArgumentException("Failed to retrieve page number.", nameof(pageNumberWithPrefix));
    }

    private string[] CreateSequenceOfPages(string lineStartsWith, int pageNumberingStartsWith, int pagesAvailable)
    {
        if (!lineStartsWith.All(x => char.IsLetter(x)))
            throw new ArgumentException("String should consist of the letter.", nameof(lineStartsWith));

        if (pageNumberingStartsWith == 0) return Array.Empty<string>();

        return Enumerable.Range(pageNumberingStartsWith, pagesAvailable).Select(x => $"{lineStartsWith} {x}").ToArray();
    }

    private void OnShowMessageBox(object? source, ApplicationInfoEventArgs args)
    {
        var msg = new StringBuilder(args.Message);
        var title = !string.IsNullOrWhiteSpace(args.Title) ? args.Title : "Ooops...";

        if (args.Ex is Exception ex)
        {
            if (msg.Length > 0) msg.AppendLine(new string('-', 50));
            msg.AppendLine(ex.Message);
        }

        MessageBox.Show(msg.ToString(), title, MessageBoxButtons.OK);
    }

    private void OnChangeTitle()
    {
        var separator = "SHILY ";
        var endOf = new List<string> { "ಠ_ಠ", "(¬_¬ )", "ಠ▃ಠ", "ಠಿ_ಠ", "⚆_⚆", "( •̀ ω •́ )y", "(╹ڡ╹ )", "(¬‿¬)", "(⊙﹏⊙)", "(⊙ˍ⊙)", "⊙﹏⊙∥", ",,ԾㅂԾ,,", "(。﹏。)", "(⊙_⊙)？", "ಠ╭╮ಠ" };
        var title = this.Text.Split(new string[] { separator }, StringSplitOptions.None);
        this.Text = $"{title.FirstOrDefault()}{separator}{endOf[new Random().Next(endOf.Count)]}";
    }
}