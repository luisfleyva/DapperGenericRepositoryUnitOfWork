using System;
using System.Collections.Generic;
using System.Text;

namespace DapperGenericRepositoryUnitOfWork.Model
{
    public class Cat
    {
        public int Id { get; set; }
        public int BreedId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
