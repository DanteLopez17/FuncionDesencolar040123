using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuncionDesencolar040123
{
    public interface IServicio
    {
        public Task GetQueues(CancellationToken stoppingToken);
    }
}
