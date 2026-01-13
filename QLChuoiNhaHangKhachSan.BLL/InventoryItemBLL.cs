using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QLChuoiNhaHangKhachSan.DAL;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.BLL
{
    public class InventoryItemBLL
    {
        private readonly EquipmentDAL _equipmentDal;
        private readonly IngredientDAL _ingredientDal;

        public InventoryItemBLL(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string is required", nameof(connectionString));
            }

            _equipmentDal = new EquipmentDAL(connectionString);
            _ingredientDal = new IngredientDAL(connectionString);
        }

        public CreateItemResult CreateItem(WarehouseType type, string name, string unit, decimal defaultPrice, decimal stockQuantity, decimal minStock)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Ten mat hang khong hop le", nameof(name));
            if (string.IsNullOrWhiteSpace(unit)) throw new ArgumentException("Don vi khong hop le", nameof(unit));
            if (defaultPrice < 0) throw new ArgumentOutOfRangeException(nameof(defaultPrice));
            if (stockQuantity < 0) throw new ArgumentOutOfRangeException(nameof(stockQuantity));
            if (minStock < 0) throw new ArgumentOutOfRangeException(nameof(minStock));

            switch (type)
            {
                case WarehouseType.Equipment:
                    var equipment = _equipmentDal.CreateEquipment(name, unit, defaultPrice, stockQuantity, minStock);
                    return new CreateItemResult
                    {
                        NewId = equipment.NewId,
                        NewCode = equipment.NewCode,
                        Type = WarehouseType.Equipment
                    };

                case WarehouseType.Ingredient:
                    var ingredient = _ingredientDal.CreateIngredient(name, unit, defaultPrice, stockQuantity, minStock);
                    return new CreateItemResult
                    {
                        NewId = ingredient.NewId,
                        NewCode = ingredient.NewCode,
                        Type = WarehouseType.Ingredient
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Loai kho khong hop le");
            }
        }

        public IList<InventoryItemDetailDTO> GetEquipmentList()
            => MapSummary(_equipmentDal.GetActiveEquipmentList(),
                          WarehouseType.Equipment,
                          "EquipmentID", "EquipmentCode", "EquipmentName");

        public IList<InventoryItemDetailDTO> GetIngredientList()
            => MapSummary(_ingredientDal.GetActiveIngredientList(),
                          WarehouseType.Ingredient,
                          "IngredientID", "IngredientCode", "IngredientName");

        public InventoryItemDetailDTO GetEquipmentById(int id)
            => MapDetail(_equipmentDal.GetEquipmentById(id),
                         WarehouseType.Equipment, "EquipmentID", "EquipmentCode", "EquipmentName");

        public InventoryItemDetailDTO GetIngredientById(int id)
            => MapDetail(_ingredientDal.GetIngredientById(id),
                         WarehouseType.Ingredient, "IngredientID", "IngredientCode", "IngredientName");

        public void UpdateEquipment(InventoryItemDetailDTO dto)
        {
            ValidateUpdateDto(dto);
            _equipmentDal.UpdateEquipment(dto.Id, dto.Name, dto.Unit,
                dto.DefaultPrice, dto.StockQuantity, dto.MinStock);
        }

        public void UpdateIngredient(InventoryItemDetailDTO dto)
        {
            ValidateUpdateDto(dto);
            _ingredientDal.UpdateIngredient(dto.Id, dto.Name, dto.Unit,
                dto.DefaultPrice, dto.StockQuantity, dto.MinStock);
        }

        public void StopItem(WarehouseType type, int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            switch (type)
            {
                case WarehouseType.Equipment:
                    _equipmentDal.StopEquipment(id);
                    break;
                case WarehouseType.Ingredient:
                    _ingredientDal.StopIngredient(id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Loai kho khong hop le");
            }
        }

        private void ValidateUpdateDto(InventoryItemDetailDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Id <= 0) throw new ArgumentOutOfRangeException(nameof(dto.Id));
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Ten mat hang khong hop le", nameof(dto.Name));
            if (string.IsNullOrWhiteSpace(dto.Unit)) throw new ArgumentException("Don vi khong hop le", nameof(dto.Unit));
            if (dto.DefaultPrice < 0) throw new ArgumentOutOfRangeException(nameof(dto.DefaultPrice));
            if (dto.StockQuantity < 0) throw new ArgumentOutOfRangeException(nameof(dto.StockQuantity));
            if (dto.MinStock < 0) throw new ArgumentOutOfRangeException(nameof(dto.MinStock));
        }

        private static IList<InventoryItemDetailDTO> MapSummary(DataTable dt, WarehouseType type,
            string idColumn, string codeColumn, string nameColumn)
        {
            if (dt == null || dt.Rows.Count == 0) return new List<InventoryItemDetailDTO>();

            return dt.AsEnumerable()
                .Select(row => new InventoryItemDetailDTO
                {
                    Id = row.Field<int>(idColumn),
                    Code = row.Field<string>(codeColumn),
                    Name = row.Field<string>(nameColumn),
                    Type = type
                })
                .ToList();
        }

        private static InventoryItemDetailDTO MapDetail(DataTable dt, WarehouseType type,
            string idColumn, string codeColumn, string nameColumn)
        {
            if (dt == null || dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new InventoryItemDetailDTO
            {
                Id = row.Field<int>(idColumn),
                Code = row.Field<string>(codeColumn),
                Name = row.Field<string>(nameColumn),
                Unit = row.Field<string>("Unit"),
                DefaultPrice = row.Field<decimal>("DefaultPrice"),
                StockQuantity = row.Field<decimal>("StockQuantity"),
                MinStock = row.Field<decimal>("MinStock"),
                Type = type
            };
        }
    }
}
