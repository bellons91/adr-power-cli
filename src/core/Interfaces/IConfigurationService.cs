using Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IConfigurationService
    {
        Task InitializeAsync(Initialization.AdrSettings settings, CancellationToken cancellationToken);
    }
}
