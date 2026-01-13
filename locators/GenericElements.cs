namespace zCustodiaUi.locators
{
    public class GenericElements
    {
        public string ButtonNew { get; } = "//span[text()='Novo']";
        public string Filter { get; } = "#z-select-filter-input";
        public string DayValue(string day) => $"//td[@role='gridcell']//button//span[text()=' {day} ']";
        public string TabAllForms(string form) => $"//span[text()=' {form} ']";
        public string RightArrow { get; } = "(//div[@class='mat-mdc-tab-header-pagination-chevron'])[2]";
        public string ReceiveTypeOption(string option) => $"//span[text()=' {option} ']";
        public string LocatorMatLabel(string LocatorName) => $"//mat-label[text()='{LocatorName}']";
        public string AddButton { get; } = "//span[text()='Adicionar']";
        public string SuccessMessage { get; } = "//div[contains(text(),'sucesso')]";
        public string SaveButton { get; } = "//span[text()='Salvar']";
        public string FilterButton { get; } = "//button//mat-icon[text()=' filter_alt ']";
        public string ImportButton { get; } = "//span[text()='Importar']";
        public string LocatorSpanText(string spanTextOption) => $"//span[text()='{spanTextOption}']";
        public string Id(string IdOption) => $"#{IdOption}";
        public string LocatorMatIcon(string MatIconOption) => $"//mat-icon[text()=' {MatIconOption} ']";
        public string EditButton { get; } = "(//mat-icon[text()=' edit_note '])[1]";
        public string AttachFileInput { get; } = "#file-input-playwright";
        public string Calendar { get; } = "//button[@aria-label='Open calendar']";
        public string RadioButtonOption(string option) => $"//input[@value='{option}']";




    }
}
