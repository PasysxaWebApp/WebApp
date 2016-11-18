namespace Pasys.Web.Admin.UI.Models.Base
{
    public interface IJqGridModel
    {
        int GridUniqueID { get; }
        object[] GridFields { get; }
    }
}