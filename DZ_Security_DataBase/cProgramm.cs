namespace Festival_Manager
{
    internal static class cProgramm
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Erstellen Sie eine Instanz des Hauptformulars
            cLoginMenu mainForm = new();

            // Erstellen Sie eine Instanz des benutzerdefinierten ApplicationContext
            CustomApplicationContext context = new(mainForm);

            // Übergeben Sie den ApplicationContext an Application.Run
            Application.Run(context);
        }

        public class CustomApplicationContext : ApplicationContext
        {
            public CustomApplicationContext(Form mainForm) : base(mainForm)
            {
            }

            protected override void OnMainFormClosed(object sender, EventArgs e)
            {
                if (Application.OpenForms.Count > 0)
                {
                    MainForm = Application.OpenForms[0];
                }
                else
                {
                    base.OnMainFormClosed(sender, e);
                }
            }
        }

    }
}