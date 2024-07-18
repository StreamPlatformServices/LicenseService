namespace LicenseService.Models
{
    //TODO: Z wyjatkami podejscie jest o tyle lepsze, ze mozna status razem z wiadomoscia przekazywac, ale teoretycznie jakby tutaj byla taka potrzeba to tez moznabyloby strukturke zrobic
    public enum ResultStatus
    {
        AccessDenied,
        BadInput,
        NotFound,
        Conflict,
        Failed,
        Success,
    }
}
