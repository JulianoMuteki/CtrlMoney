using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Interfaces.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.AppService
{
    public class RegisterAppService : IRegisterAppService
    {
        IGrandChildTreeAppService _grandChildTreeAppService;
        IChildTreeAppService _childTreeAppService;
        IParentTreeAppService _parentTreeAppService;
        public RegisterAppService(IGrandChildTreeAppService grandChildTreeAppService, IChildTreeAppService childTreeAppService, IParentTreeAppService parentTreeAppService)
        {
            _childTreeAppService = childTreeAppService;
            _grandChildTreeAppService = grandChildTreeAppService;
            _parentTreeAppService = parentTreeAppService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeID">EMPTY</param>
        /// <param name="titleTree"></param>
        /// <param name="description"></param>
        /// <param name="tag"></param>
        public void AddTree(string treeID, string titleTree, string description, string tag)
        {
            if (string.IsNullOrEmpty(treeID))
            {
                _parentTreeAppService.Add(new ParentTree()
                {
                    Title = titleTree,
                    Description = description,
                    Tag = tag
                });
            }
            else
            {
                Guid id = new Guid(treeID);
                var parentTree = _parentTreeAppService.GetById(id);
                if (parentTree != null)
                {
                    _childTreeAppService.Add(new ChildTree()
                    {
                        Title = titleTree,
                        Description = description,
                        Tag = tag,
                        ParentNodeID = parentTree.Id
                    });
                }
                else
                {
                    var child = _childTreeAppService.GetById(id);
                    if (child != null)
                    {
                        _grandChildTreeAppService.Add(new GrandChildTree()
                        {
                            Title = titleTree,
                            Description = description,
                            Tag = tag,
                            ParentNodeID = child.Id
                        });
                    }
                    else
                    {
                        throw new Exception("Can't add tree bellow grand tree");
                    }
                }
            }
        }

        public ICollection<ParentTree> GetAll()
        {
            return _parentTreeAppService.GetAll();
        }
    }
}
