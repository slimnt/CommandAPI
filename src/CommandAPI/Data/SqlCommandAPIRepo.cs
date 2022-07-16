using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Models;

namespace CommandAPI.Data
{
	public class SqlCommandAPIRepo : ICommandAPIRepo
	{
		private readonly CommandContext _context;

		public SqlCommandAPIRepo(CommandContext context)
		{
			_context = context;
		}

		public void CreateCommand(Command command)
		{
			throw new NotImplementedException();
		}

		public void DeleteCommand(Command command)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Command> GetAllCommands()
		{
			return _context.CommandItems.ToList();
		}

		public Command GetCommandById(int id)
		{
			return _context.CommandItems.FirstOrDefault(item => item.Id == id);
		}

		public bool SaveChanges()
		{
			throw new NotImplementedException();
		}

		public void UpdateCommand(Command command)
		{
			throw new NotImplementedException();
		}
	}
}