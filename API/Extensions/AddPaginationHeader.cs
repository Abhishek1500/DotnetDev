using System.Text.Json;
using API.Helper;

namespace API.Extensions;
//learn this header and stuff
public static class HttpExtensions{
//the reason of using this serializer option is to make the entities json compatible
//beacuse the header will contain json data
    public static void AddPaginationHeader(this HttpResponse response,PaginationHeader header){
        //telling to change the case
        var jsonOptions=new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
        response.Headers.Add("Pagination",JsonSerializer.Serialize(header,jsonOptions));
        response.Headers.Add("Access-Control-Expose-Header","Pagination");
    }

}