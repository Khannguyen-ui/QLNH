using System.Data;

namespace GUI_QLNH.Common
{
    // BẮT BUỘC PHẢI LÀ "public interface"
    public interface IExportable
    {
        string ExportKey { get; }
        DataTable GetExportData();
        string DefaultExportFileName { get; }
    }
}