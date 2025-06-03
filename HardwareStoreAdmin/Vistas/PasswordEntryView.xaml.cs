namespace HardwareStoreAdmin.Vistas;

public partial class PasswordEntryView : ContentView
{
    public Border EntryBorderPublic => ContrasenaBorder;
    public Label LabelPublic => ContraseñaErrorLabel;
    public Entry PasswordEntryControl => PasswordEntry;

    public PasswordEntryView()
    {
        InitializeComponent();
        BindingContext = this;
    }

    // Texto que aparece como etiqueta (ej: "Contraseña", "Confirmar contraseña")
    public static readonly BindableProperty LabelTextProperty =
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(PasswordEntryView), default(string));

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    // Placeholder que aparece dentro del Entry
    public static readonly BindableProperty PlaceholderTextProperty =
        BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(PasswordEntryView), default(string));

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    // Texto real de la contraseña
    public static readonly BindableProperty PasswordProperty =
        BindableProperty.Create(nameof(Password), typeof(string), typeof(PasswordEntryView), default(string), BindingMode.TwoWay);

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    private bool _isPasswordVisible = false;
    public bool IsPassword => !_isPasswordVisible;

    public ImageSource IconSource =>
        _isPasswordVisible
        ? new FontImageSource { Glyph = Iconos.Iconos.ojoAbierto, FontFamily = "Iconos", Color = Colors.Gray }
        : new FontImageSource { Glyph = Iconos.Iconos.ojoCerrado, FontFamily = "Iconos", Color = Colors.Gray };


    private void OnEyeIconClicked(object sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        OnPropertyChanged(nameof(IsPassword));
        OnPropertyChanged(nameof(IconSource));
    }
    private void OnPasswordEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        ContrasenaBorder.Stroke = Colors.Gray;
        ContraseñaErrorLabel.IsVisible = false;
    }
    public void Clear()
    {
        Password = string.Empty;
        PasswordEntry.Text = string.Empty;
    }

}