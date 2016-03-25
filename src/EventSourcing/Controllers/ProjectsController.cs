using System.Web.Http;

namespace EventSourcing.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : ApiController
    {
        [Route("{id}")]
        public Project Get(int id)
        {
            return new Project {id = 1, name = "Heart Beat"};
        }
    }

    public class Project
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
