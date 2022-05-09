namespace ParserOfPsychologists.WinFormsUI;

public partial class MainForms : Form
{
    private readonly IStateOfCityModule _stateOfCity;
    private readonly IParserWebRequestsFacade _facade;

    public MainForms(IParserWebRequestsFacade facade, IStateOfCityModule stateOfCityModule)
    {
        _facade = facade;
        _stateOfCity = stateOfCityModule;

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
            if (s is ComboBox box && _stateOfCity.IsChanged(box.Text)) await _stateOfCity.ChangeCityAsync(box.Text);
        };
    }
}