using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    /* ChatGPT
     * Código inserido devido crash ao deletar valor ou tentar inserir negativo nos campos 
     * Quantidade ou Preco. Com Binding no Entry, double dava erro.
     * Preenche os campos da tela com os dados do produto quando a página aparece
     * (abre ou volta da navegação - PopAsync)*/
    
    protected override void OnAppearing()
    {
        base.OnAppearing(); //Executa o comportamento padrão da classe ContentPage, garante funcionamento do Maui

        if (BindingContext is Produto p) //Se BindingContext é um Produto, ele faz o cast automaticamente e cria a variável p
        {
            txt_descricao.Text = p.Descricao;
            txt_quantidade.Text = p.Quantidade.ToString(); //Converte double para string
            txt_preco.Text = p.Preco.ToString(); //Converte double para string
            txt_categoria.Text = p.Categoria;
        }
    }
    
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            //Variável p do tipo Produto recebe um novo objeto do tipo Produto.
            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = txt_categoria.Text
            };

            //A partir do Db temos acesso aos métodos implementados na SQLiteDataBaseHelper
            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
            await Navigation.PopAsync();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}