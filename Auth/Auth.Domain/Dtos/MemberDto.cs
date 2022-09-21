namespace Auth.Domain.Dtos
{
    public class MemberDto : UserDto
    {
        public double Height { get; set; }
        public double Weight { get; set; }
    }
}
