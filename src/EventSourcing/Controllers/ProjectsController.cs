using System;
using System.Web.Http;
using EventSourcing.Contracts;
using EventSourcing.Contracts.Dtos;
using EventSourcing.Contracts.Events;

namespace EventSourcing.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : ApiController
    {
        private readonly IRepository _repository;

        public ProjectsController()
        {
            _repository = IoC.GesRepository;
        }

        [Route("{id}")]
        public Project Get(int id)
        {
            return new Project {id = Guid.NewGuid(), name = "Heart Beat"};
        }

        [Route(""), HttpPost]
        public Guid Add(AddProjectReq req)
        {
            var id = Guid.NewGuid();
            var e = new ProjectAdded
            {
                Id = id,
                Name = req.Name
            };
            _repository.Save(id, new [] {e});
            return id;
        }
    }

    public class AddProjectReq
    {
        public string Name { get; set; }
    }
}
