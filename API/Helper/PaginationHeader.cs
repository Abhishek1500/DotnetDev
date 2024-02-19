using System.Security.Cryptography.X509Certificates;

namespace API.Helper;

//here this pagination header is to be send back to
//client to give the info regarding pagination and for that we created extension method in extension folder
public class PaginationHeader{

    public PaginationHeader(int currentPage,int itemPerPage,int totalItems,int totalPages){
        CurrentPage=currentPage;
        ItemPerPage=itemPerPage;
        TotalItems=totalItems;
        TotalPages=totalPages;
    } 

    public int CurrentPage {get;set;}
    public int ItemPerPage {get;set;}

    public int TotalItems {get;set;}

    public int TotalPages {get; set;}

}