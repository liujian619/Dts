using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Dts
{
	internal sealed class MemberCollection : Collection<Member>
	{
		public MemberCollection() { }

		public MemberCollection(IEnumerable<Member> members)
		{
			Init(members);
		}

		/// <summary>
		/// 渲染所有成员。
		/// </summary>
		/// <returns></returns>
		public string Render()
		{
			StringBuilder sb = new();
			foreach (var item in Items)
			{
				sb.AppendLine(item.Render());
				sb.AppendLine();
			}

			return sb.ToString();
		}

		private void Init(IEnumerable<Member> members)
		{
			Stack<MemberCollection> stack = new();
			List<Member> typeList = new();
			
			foreach (var item in members)
			{
				MemberCollection top = stack.TryPeek(out MemberCollection? r) && r is not null ? r : this;

				if (item.Type == MemberType.Type)
				{
					// type 类型定义只能加到最顶层
					item.Level = 0;
					typeList.Add(item);
				}
				else if (item.Type == MemberType.Raw)
				{
					item.Level = stack.Count;
					top.Add(item);
				}
				else if (item.Type == MemberType.Interface || item.Type == MemberType.Class)
				{
					item.Level = stack.Count;
					top.Add(item);
					stack.Push(item.Children);
				}
				else if (item.Type == MemberType.Method || item.Type == MemberType.Property)
				{
					item.Level = stack.Count;
					top.Add(item);
				}
				else
				{
					stack.TryPop(out _);
				}
			}

			foreach (var t in typeList)
			{
				Insert(0, t);
			}
		}
	}
}
