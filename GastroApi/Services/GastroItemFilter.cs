using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
using SqlKata.Execution;
using SqlKata;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using GastroApi.Models;
using Newtonsoft.Json;
using Microsoft.Build.Framework;
using System.Runtime.CompilerServices;

namespace GastroApi.Services
{

public interface ISpecification
{
    Query ApplySpecification(Query query);
}

public class RecipeSpecification : ISpecification
{
    private string _recipe;

    public RecipeSpecification(string recipe){
        _recipe = recipe;
    }

    public Query ApplySpecification(Query query){
        return query.WhereRaw("(data ->> 'Recipe') :: text ILIKE ? ", $"%{_recipe}%");
    }
}

public class DescriptionSpecification : ISpecification
{
    private string _description;

    public DescriptionSpecification(string description){
        _description = description;
    }

    public Query ApplySpecification(Query query){
        return query.WhereRaw("(data ->> 'DescriptionName') :: text ILIKE ? ", $"%{_description}%");
    }
}

public class OrSpecification : ISpecification
{
    private ISpecification _left;
    private ISpecification _right;

    public OrSpecification(ISpecification left, ISpecification right)
    {
        _left = left;
        _right = right; 
    }

    public Query ApplySpecification(Query query)
    {
        return query.Where( q => 
        q.Where(sq => _left.ApplySpecification(sq))
        .OrWhere(sq => _right.ApplySpecification(sq)));
    }
}
 public class DescriptionOrRecipe : ISpecification
    {
        private OrSpecification _orSpec;
        public DescriptionOrRecipe(string description, string recipe)
        {
            _orSpec = new OrSpecification(
                new DescriptionSpecification(description),
                new RecipeSpecification(recipe)
            );
        }
        public Query ApplySpecification(Query query)
        {   
            return _orSpec.ApplySpecification(query);            
        }
    }






}