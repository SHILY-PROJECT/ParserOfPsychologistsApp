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
    }

    private void ActionOnEventsToLoadAndCloseForm()
    {
        var cities = async () => (await _facade.FindCityAsync(string.Empty)).ToArray();
        this.Load += async (s, e) => this.citiesBox.Items.AddRange(await cities.Invoke());
    }
}