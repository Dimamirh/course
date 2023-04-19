using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Controls;
using static Android.Print.PrintAttributes;

namespace course_project_nosov;

public partial class MainPage : ContentPage
{
    MyDataContext db = new MyDataContext();
    private object usersList;

    public ObservableCollection <User> DataContext { get; set; }
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        // при загрузке окна
        private void MainPage_Loaded(object sender, EventArgs e)
        {
            // гарантируем, что база данных создана
            db.Database.EnsureCreated();
            // загружаем данные из БД
            db.Users.Load();
            // и устанавливаем данные в качестве контекста
            using (var db = new MyDataContext())
            {
                DataContext = db.Users.Local.ToObservableCollection();
            }
            //DataContext = db.Users.Local.ToObservableCollection();
        }

        // добавление
        private void Add_Click(object sender, EventArgs e)
        {
            UserWindow UserWindow = new UserWindow(new User());
            if (UserWindow.ShowDialog() == true)
            {
                User User = UserWindow.User;
                db.Users.Add(User);
                db.SaveChanges();
            }
        }
        // редактирование
        private void Edit_Click(object sender, EventArgs e)
        {
        // получаем выделенный объект
        // если ни одного объекта не выделено, выходим
        if (usersList.SelectedItem is not User user) return;

        UserWindow UserWindow = new UserWindow(new User
            {
                Id = user.Id,
                Age = user.Age,
                Name = user.Name
            });

            if (UserWindow.ShowDialog() == true)
            {
                // получаем измененный объект
                user = db.Users.Find(UserWindow.User.Id);
                if (user != null)
                {
                    user.Age = UserWindow.User.Age;
                    user.Name = UserWindow.User.Name;
                    db.SaveChanges();
                    usersList.Items.Refresh();
                }
            }
        }
        // удаление
        private void Delete_Click(object sender, EventArgs e)
        {
            // получаем выделенный объект
            User? user = usersList.SelectedItem as User;
            // если ни одного объекта не выделено, выходим
            if (user is null) return;
            db.Users.Remove(user);
            db.SaveChanges();
        }
    
}
	/*int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}*/
