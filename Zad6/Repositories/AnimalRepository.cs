using System.Data.SqlClient;

namespace Zad6.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private IConfiguration _configuration;

    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Animal> GetAnimals(string orderBy)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        string order;
        //oddzielna metoda
        switch(orderBy.ToLower())
        {
            case "description":
                order = "DESCRIPTION";
                break;
            case "category":
                order = "CATEGORY";
                break;
            case "area":
                order = "AREA";
                break;
            default:
                order = "NAME";
                break;
        }

        cmd.CommandText = "SELECT * FROM ANIMAL ORDER BY " + order;

        var dr = cmd.ExecuteReader();
        List<Animal> animals = new List<Animal>();
        while (dr.Read())
        {
            var animal = new Animal
            {
                IdAnimal = (int)dr["IdAnimal"],
                Name = dr["Name"].ToString(),
                Description = dr["Description"].ToString(),
                Category = dr["Category"].ToString(),
                Area = dr["Area"].ToString()

            };
            animals.Add(animal);
        }

        return animals;
    }

    public int CreateAnimal(AnimalDTO animalDTO)
    {
        (string name, string description, string category, string area) = animalDTO;
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO ANIMAL(Name,Description,Category,Area) VALUES(@Name,@Description,@Category,@Area)";
        
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@Category", category);
        cmd.Parameters.AddWithValue("@Area", area);

        return cmd.ExecuteNonQuery();
    }

    public int DeleteAnimal(int idAnimal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "DELETE FROM ANIMAL WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", idAnimal);
        return cmd.ExecuteNonQuery();
    }   
    public int UpdateAnimal(Animal animal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "UPDATE ANIMAL SET Name=@Name, Description=@Description,Category=@Category,Area=@Area WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@Name", animal.Name);
        cmd.Parameters.AddWithValue("@Description", animal.Description);
        cmd.Parameters.AddWithValue("@Category", animal.Category);
        cmd.Parameters.AddWithValue("@Area", animal.Area);
        cmd.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);

        return cmd.ExecuteNonQuery();
    }
}