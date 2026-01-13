using System.Data;
using QLChuoiNhaHangKhachSan.DAL;

namespace QLChuoiNhaHangKhachSan.BLL
{
    public class IngredientBLL
    {
        private readonly IngredientDAL _ingredientDal;

        public IngredientBLL(string connectionString)
        {
            _ingredientDal = new IngredientDAL(connectionString);
        }

        public DataTable GetIngredientsForCombo()
        {
            return _ingredientDal.GetActiveIngredientsForCombo();
        }

        public DataTable GetIngredientsForExport()
        {
            return _ingredientDal.GetActiveIngredientsForExport();
        }

        public DataTable GetIngredientById(int id)
        {
            return _ingredientDal.GetIngredientById(id);
        }
    }
}
