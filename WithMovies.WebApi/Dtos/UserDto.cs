namespace WithMovies.WebApi.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; }
        public int Reviews { get; set; }
        public string LastOnline { get; set; }
        public bool IsBlocked { get; set; }
    }
}
