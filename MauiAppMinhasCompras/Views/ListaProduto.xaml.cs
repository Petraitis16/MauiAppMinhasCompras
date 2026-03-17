using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    /*Permite que a coleção notifique automaticamente a interface do usuário sobre alterações na sua estrutura,
     * como adição, remoção ou modificação de itens.*/
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    protected async override void OnAppearing()
    {
        try
        {
            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }


	//Clicked de Adicionar Produto - direciona para nova tela
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
    }

    //Evento do SearchBar: Textchanged - busca dinâmica atualiza a interface conforme o texto for alterado.
	private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lista.Clear();

            /*List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));*/

            //Evitar consultas se campo de busca estiver vazio
            List<Produto> tmp;

            if (string.IsNullOrWhiteSpace(q))
                tmp = await App.Db.GetAll();
            else
                tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));

            //Mensagem para o caso de nenhum produto ser encontrado
            if (tmp.Count == 0)
                lbl_sem_resultado.IsVisible = true;
            else
                lbl_sem_resultado.IsVisible = false;
            lst_produtos.IsVisible = tmp.Count > 0;
            lbl_sem_resultado.IsVisible = tmp.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);

            string msg = $"O total é {soma:C}";

            DisplayAlert("Total dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem; //Typecast; ao clicar em MenuItem, chegará qual foi selecionado.

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert("Tem certeza?", $"Remover {p.Descricao}", "Sim", "Não");

            if(confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p); //Retira da ObservableCollection e ListView.
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}