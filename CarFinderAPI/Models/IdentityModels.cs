using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CarFinderAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Cars> Car { get; set; }

        //============================================================================================
        //=============================== Stored Procedures ==========================================
        //============================================================================================

        ////=============================== Car by Year ==========================================

        //public async Task<List<Cars>> GetCarByYear (string Year)
        //{
        //    return await this.Database.SqlQuery<Cars>("CarByYear @Year", new SqlParameter("year", Year) ).ToListAsync();
        //}

        ////=============================== Get all by Engine Position ==========================================

        //public async Task<List<Cars>> GetCarByEnginePosition (string Position)
        //{
        //    return await this.Database.SqlQuery<Cars>("GetAllByEnginePosition @position", new SqlParameter("position", Position) ).ToListAsync();
        //}

        ////=============================== Get all by Weight ==========================================

        //public async Task<List<Cars>> GetAllByWeight(string weight)
        //{
        //    return await this.Database.SqlQuery<Cars>("GetAllByWeight @weight  ", new SqlParameter("weight ", weight  ) ).ToListAsync();
        //}

        

        ////=============================== Get all Greater than 300 HP ==========================================

        //public async Task<List<Cars>> GetAllGreaterThan300hp ()
        //{
        //    return await this.Database.SqlQuery<Cars>("GetAllGreaterThan300HP").ToListAsync();
        //}
        ////=============================== Get all OrderByHp ==========================================

        //public async Task<List<Cars>> GetAllOrderByHp ()
        //{
        //    return await this.Database.SqlQuery<Cars>("GetAllOrderByHP  ").ToListAsync();
        //}

        ////=============================== Get all SUV ==========================================

        //public async Task<List<Cars>> GetAllSUV()
        //{
        //    return await this.Database.SqlQuery<Cars>("GetAllSUV" ).ToListAsync();
        //}

        ////=============================== Get all weight less than 200 KG ==========================================

        //public async Task<List<Cars>> GetAllWeightLessThan200kg ()
        //{
        //    return await this.Database.SqlQuery<Cars>("GetAllWeightLessThan200kg" ).ToListAsync();
        //}

        ////=============================== Get all weight less than 200 KG More than 300HP ==========================================

        //public async Task<List<Cars>> GetAllWeightLessThan200kgMoreThan300hp ()
        //{
        //    return await this.Database.SqlQuery<Cars>("GetAllWeightLessThan200kgMoreThan300hp").ToListAsync();
        //}

        //=============================== Get Unique Model Years ==========================================

        public async Task<List<string>> GetUniqueModelYears()
        {
            return await this.Database.SqlQuery<string>("GetUniqueModelYears").ToListAsync();
        }

        //=============================== Get Make By Year ==========================================

        public async Task<List<string>> GetMakeByYear(string Year)
        {
            return await this.Database.SqlQuery<string>("GetMakeByYear @Year", new SqlParameter("Year", Year)).ToListAsync();
        }
        //=============================== Get Model By Year and Make ==========================================

        public async Task<List<string>> GetModelByYearAndMake(string Year, string Make)
        {
            return await this.Database.SqlQuery<string>("GetModelByYearAndMake @Year, @Make",
                new SqlParameter("Year", Year), new SqlParameter("Make", Make)).ToListAsync();
        }

        //=============================== Get Trims By Year Make Model ==========================================

        public async Task<List<string>> GetTrimsByYearMakeModel (string Year, string Make, string Model)
        {
            return await this.Database.SqlQuery<string>("GetTrimsByYearMakeModel @Year, @Make, @Model",
                new SqlParameter("Year", Year), new SqlParameter("Make", Make), new SqlParameter("Model", Model)).ToListAsync();
        }

        //=============================== Get all by Year Make Model Trim ==========================================

        public Task<List<Cars>> GetCarsByYearMakeModelTrim(string year, string make, string model, string trim)
        {
            return this.Database.SqlQuery<Cars>("GetAllByYearMakeModelTrim @Year, @Make, @Model, @Trim",
                new SqlParameter("year", year),
                new SqlParameter("make", make),
                new SqlParameter("model", model),
                new SqlParameter("trim", trim)).ToListAsync();
        }

        ////=============================== Get Unique Makes ==========================================

        //public async Task<List<string>>GetUniqueMakes()
        //{
        //    return await this.Database.SqlQuery<string>("GetUniqueMakes").ToListAsync();
        //}

        ////=============================== Get Unique Models ==========================================

        //public async Task<List<string>>GetUniqueModels()
        //{
        //    return await this.Database.SqlQuery<string>("GetUniqueModels").ToListAsync();
        //}



        ////=============================== Get Unique Trims ==========================================

        //public async Task<List<string>> GetUniqueTrims()
        //{
        //    return await this.Database.SqlQuery<string>("GetUniqueTrims").ToListAsync();
        //}



    }
}