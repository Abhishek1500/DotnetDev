using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace API.Helper;

//here the class is derived from the List type so contain all power of list and now adding the power of pagination
//the querable is differed that means when called then query will run 
//use count to list etc for that
//in Userrepo instead of sending the List we are going to provide pagelist
public class PageList<T> :List<T>
{
    public PageList(IEnumerable<T> items,int count,int pageNumber,int pageSize){
        CurrentPage=pageNumber;
        PageSize=pageSize;
        TotalCount=count;
        TotalPages=(int )Math.Ceiling(count/(double)pageSize);
       //study about this AddRange
        AddRange(items);
    }

    public int CurrentPage {get;set;}
    
    public int TotalPages {get;set;}
    public int PageSize {get;set;}
    public int TotalCount {get;set;}

    public static async Task<PageList<T>> CreateAsync(IQueryable<T> source,int pageNumber,int pageSize){
        var count=await source.CountAsync();
        var items=await source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
        return new PageList<T>(items,count,pageNumber,pageSize);
    }

}