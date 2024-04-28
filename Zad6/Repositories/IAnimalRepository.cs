namespace Zad6.Repositories;

public interface IAnimalRepository
{
    IEnumerable<Animal> GetAnimals(string orderBy);
    int CreateAnimal(AnimalDTO animalDTO);
    int UpdateAnimal(Animal animal);
    int DeleteAnimal(int idAnimal);
}