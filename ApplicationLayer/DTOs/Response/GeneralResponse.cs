namespace ApplicationLayer.DTOs.Response
{
    public record GeneralResponse(bool IsSuccess = false, string Message = null!);
}
