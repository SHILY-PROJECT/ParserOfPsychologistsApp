global using System;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using ParserOfPsychologists.Application.Parser;
global using ParserOfPsychologists.Application.Interfaces;
global using ParserOfPsychologists.Application.Configuration;
global using ParserOfPsychologists.Application.CustomEventArgs;
global using ParserOfPsychologists.WinFormsUI.Configuration;

namespace ParserOfPsychologists.WinFormsUI;

internal partial class WaitForm : Form
{
    private readonly IParser _parser;
    private readonly IParserSettings _parserSettings;

    public WaitForm(IParser parser, IParserSettings parserSettings)
    {
        _parser = parser;
        _parserSettings = parserSettings;

        InitializeComponent();
        RegisterFormEvents();
    }

    private void RegisterFormEvents()
    {
        ActionOnEventsToLoadAndCloseForm();
    }

    private void ActionOnEventsToLoadAndCloseForm()
    {
        this.Load += (s, e) => _parser.StateOfProgressChanged += OnStateOfProgressChanged;

        this.Load += (s, e) => this.Location = new Point(
            Owner.Location.X + Owner.Width / 2 - this.Width / 2,
            Owner.Location.Y + Owner.Height / 2 - this.Height / 2);

        this.FormClosing += (s, e) => _parser.StateOfProgressChanged -= OnStateOfProgressChanged;
    }

    private void OnStateOfProgressChanged(object? source, StateOfProgressEventArgs args) => this.Invoke(() =>
    {
        this.pageLabel.Text = $"{LabelPrefixOf(pageLabel)}{args.NumberOfPagesProcessed} из {_parserSettings.PageTo - _parserSettings.PageFrom + 1}";
        this.usersLabel.Text = $"{LabelPrefixOf(usersLabel)}{args.NumberOfUsersProcessed}";
    });

    private static string? LabelPrefixOf(Label label) => label.Text.Split(':').FirstOrDefault() + ": ";

    protected override void OnPaint(PaintEventArgs e)
    {
        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
    }
}