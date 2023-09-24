namespace dentist_project.Serivce
{
    public class Navservice
    {
        public Boolean hidenav { get; set; } = false;

        public void hide()
        {
            hidenav = true;
        }
    }
}
