using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        IProductService _productService;
        ICategoryService _categoryService;
        public Form1()
        {
            InitializeComponent();
            _productService = new ProductManager(new EfProductDal());
            _categoryService = new CategoryManager(new EfCategoryDal());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var list = _categoryService.GetAll();
            list.Insert(0, new Category { CategoryId = 0, CategoryName = "Tüm Kategoriler" });
            cbxCategory.DataSource = list;
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";
            cbxCategory.SelectedIndex = 0;

            cbxCategoryIdAdd.DataSource = _categoryService.GetAll();
            cbxCategoryIdAdd.ValueMember = "CategoryId";
            cbxCategoryIdAdd.DisplayMember = "CategoryName";
            cbxCategoryIdAdd.Text = "Kategori Seçin";
        }

        private void LoadCategoryUpdate()
        {
            cbxCategoryIdUpdate.DataSource = _categoryService.GetAll();
            cbxCategoryIdUpdate.ValueMember = "CategoryId";
            cbxCategoryIdUpdate.DisplayMember = "CategoryName";
            cbxCategoryIdUpdate.SelectedValue = dgwProduct.CurrentRow.Cells[2].Value;
        }

        private void LoadProducts()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCategory.SelectedIndex == 0)
                LoadProducts();
            else
                dgwProduct.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            string key = tbxSearch.Text;
            if (rbtnAllProducts.Checked)
            {
                LoadCategories();
                dgwProduct.DataSource = _productService.SearchProductsByName(key);
            }
            else if (rbtnSelectedCategoryProducts.Checked)
            {
                int categoryId = Convert.ToInt32(cbxCategory.SelectedValue);
                dgwProduct.DataSource = _productService.SearchProductsByNameInSelectedCategory(key, categoryId);
            }

        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadCategoryUpdate();
            tbxProductNameUpdate.Text = dgwProduct.CurrentRow.Cells[1].Value.ToString();
            tbxUnitPriceUpdate.Text = dgwProduct.CurrentRow.Cells[3].Value.ToString();
            tbxQuantityPerUnitUpdate.Text = dgwProduct.CurrentRow.Cells[4].Value.ToString();
            tbxUnitsInStockUpdate.Text = dgwProduct.CurrentRow.Cells[5].Value.ToString();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var product = new Product
            {
                ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value)
            };
            try
            {
                _productService.Delete(product);
                LoadProducts();
                MessageBox.Show("Ürün Silindi!");
                ClearUpdateTextboxes();
            }
            catch (Exception)
            {

                MessageBox.Show(
                    "Ürün silinemedi, ürünün aktif veya geçmiş siparişlerinin olması genellikle buna engeldir.");
            }
            
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var product = new Product
            {
                ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
                ProductName = tbxProductNameUpdate.Text,
                CategoryId = Convert.ToInt32(cbxCategoryIdUpdate.SelectedValue),
                QuantityPerUnit = tbxQuantityPerUnitUpdate.Text,
                UnitPrice = Convert.ToDecimal(tbxUnitPriceUpdate.Text),
                UnitsInStock = Convert.ToInt16(tbxUnitsInStockUpdate.Text)
            };

            _productService.Update(product);
            LoadCategoryUpdate();
            LoadProducts();
            MessageBox.Show("Ürün güncellendi.");
            ClearUpdateTextboxes();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var product = new Product
                {
                    ProductName = tbxProductNameAdd.Text,
                    CategoryId = Convert.ToInt32(cbxCategoryIdAdd.SelectedValue),
                    QuantityPerUnit = tbxQuantityPerUnitAdd.Text,
                    UnitPrice = Convert.ToDecimal(tbxUnitPriceAdd.Text),
                    UnitsInStock = Convert.ToInt16(tbxUnitsInStockAdd.Text)
                };
                _productService.Add(product);
                LoadProducts();
                MessageBox.Show("Ürün eklendi.");
                ClearAddTextboxes();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ClearAddTextboxes()
        {
            tbxProductNameAdd.Clear();
            tbxQuantityPerUnitAdd.Clear();
            tbxUnitPriceAdd.Clear();
            tbxUnitsInStockAdd.Clear();
        }

        private void ClearUpdateTextboxes()
        {
            tbxProductNameUpdate.Clear();
            tbxQuantityPerUnitUpdate.Clear();
            tbxUnitPriceUpdate.Clear();
            tbxUnitsInStockUpdate.Clear();
        }
    }
}
