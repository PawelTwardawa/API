using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using trojakty_api.Core.Exceptions;
using trojakty_api.Core.GroupService.DTOs;
using trojakty_api.Core.QuestionService;
using trojakty_api.Core.QuestionService.DTOs;
using trojkaty_api.DataAccess.Models;
using trojkaty_api.DataAccess.Repositories;

namespace trojakty_api.Core.GroupService
{
    public class GroupService : IGroupService
    {
        private IGenericRepository<Group> _groupRepository;
        private IQuestionService _questionService;
        private IGenericRepository<UserGroup> _userGroupRepository;
        private IMapper _mapper;
        private IGenericRepository<GroupQuestion> _groupQuestionRepository;

        public GroupService(IMapper mapper,IGenericRepository<GroupQuestion> groupQuestionRepository, IGenericRepository<Group> groupRepository, IGenericRepository<UserGroup> userGroupRepository, IQuestionService questionService)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
            _questionService = questionService;
            _groupQuestionRepository = groupQuestionRepository;
        }

        public async Task<Group> CreateGroupAsync(GroupDTO groupDto, User user)
        {
            if ((groupDto == null))
                throw new TrojkatyCoreException("Groups cannot be null ");

            Group group = new Group();
            group.Questions = new List<GroupQuestion>();

            foreach (var q in groupDto.Questions)
            {
                var question = await _questionService.CreateQuestionAsync(q);

                group.Questions.Add(new GroupQuestion(){ Group = group, Question = question});
            }

            group.Name = groupDto.Name;

            group.Users = new List<UserGroup>() {new UserGroup() {Group = group, User = user}};

            await _groupRepository.Add(group);
            await _groupRepository.SaveAsync();

            return group;
        }

        public async Task<List<Group>> GetGroupsAsync(User user)
        {
            return await Task.Run(() => _userGroupRepository.FindBy(x => x.User == user).Select(g => g.Group).ToList());
        }

        public async Task<Group> GetGroupAsync(int id, User user)
        {
            //return await Task.Run(() => _groupRepository.FindBy(x => x.Id == id).SingleOrDefault());
            return await Task.Run(() =>
                {
                    return _groupRepository.FindBy(x => x.Id == id).Include(x => x.Users).Where(w => w.Users.Any(x => x.User.Id == user.Id)).SingleOrDefault();
                });

        }

        public async Task<Group> InsertQuestionAsync(Group group, QuestionDTO questionDto)
        {
            if (group == null)
                throw new TrojkatyCoreException($"Cannot find group");

            return await Task.Run(async () =>
            {
                var g = _groupRepository.FindBy(x => x.Id == group.Id).Include(x => x.Questions).SingleOrDefault();
                
                var question = await _questionService.CreateQuestionAsync(questionDto);
                g.Questions.Add(new GroupQuestion() {Question = question});
                await _groupRepository.SaveAsync();

                return g;
            });
        }

        public async Task<Group> RemoveQuestionAsync(Group group, Question question)
        {
            if (group == null)
                throw new TrojkatyCoreException($"Cannot find group");

            return await Task.Run(async () =>
            {
                var g = _groupRepository.FindBy(x => x.Id == group.Id).Include(x => x.Questions).SingleOrDefault();

                var gq = _groupQuestionRepository.FindBy(x => x.GroupId == group.Id && x.QuestionId == question.Id)
                    .SingleOrDefault();

                if(gq == null)
                    throw  new TrojkatyCoreException("Cannot find question in group");

                g.Questions.Remove(gq);
                await _groupRepository.SaveAsync();

                return g;
            });
        }

        public async Task<Group> InsertUserAsync(Group group, User user)
        {
            if (group == null)
                throw new TrojkatyCoreException($"Cannot find group");

            if (user == null)
                throw new TrojkatyCoreException($"User doesn't exist");

            int count = _userGroupRepository.FindBy(x => x.UserId == user.Id && x.GroupId == group.Id).Count();

            if(count != 0)
                throw new TrojkatyCoreException($"User '{user.Email}' has access to the '{group.Name}' group");

            return await Task.Run(async () =>
            {
                var g = _groupRepository.FindBy(x => x.Id == group.Id).Include(x => x.Users).SingleOrDefault();
                if (g == null)
                    throw new TrojkatyCoreException($"Cannot find group on id {group.Id}");
                
                g.Users.Add(new UserGroup() { User = user});
                await _groupRepository.SaveAsync();

                return g;
            });
        }

        public async Task<Group> RemoveUserAsync(Group group, User user)
        {
            if (group == null)
                throw new TrojkatyCoreException($"Cannot find group");

            if (user == null)
                throw new TrojkatyCoreException($"User doesn't exist");

            int countGroups = _userGroupRepository.FindBy(x => x.GroupId == group.Id).Count();
            if (countGroups == 1)
                throw new TrojkatyCoreException($"Cannot remove last user '{user.Email}' from '{group.Name}' group");

            int countUsers = _userGroupRepository.FindBy(x => x.UserId == user.Id && x.GroupId == group.Id).Count();
            if (countUsers == 0)
                throw new TrojkatyCoreException($"Cannot remove user '{user.Email}' from '{group.Name}' group");

            return await Task.Run(async () =>
            {
                var g = _groupRepository.FindBy(x => x.Id == group.Id).Include(x => x.Users).SingleOrDefault();
                if (g == null)
                    throw new TrojkatyCoreException($"Cannot find group on id {group.Id}");

                var ug = _userGroupRepository.FindBy(x => x.UserId == user.Id && x.GroupId == group.Id)
                    .SingleOrDefault();

                g.Users.Remove(ug);
                await _groupRepository.SaveAsync();

                return g;
            });
        }

        public async Task<Group> PublishAsync(Group group) //TODO: jezeli w ktoryms pytaniu nie ma kategorii to nie upubliczniamy zadnego
        {
            if (group == null)
                throw new TrojkatyCoreException($"Cannot find group");

            //var g = _groupRepository.FindBy(x => x.Id == group.Id).Include(x => x.Questions).SingleOrDefault();
            var groupPublish = _groupRepository.FindBy(x => x.Id == group.Id).Include(x => x.Questions)
                .ThenInclude(x => x.Question).ThenInclude(x => x.Category).SingleOrDefault();

            foreach (var qg in groupPublish.Questions)
            {
                if(qg.Question.Category == null)
                    throw  new TrojkatyCoreException("Cannot publish questions without category");
                qg.Question.Public = true;
                var q = _mapper.Map<QuestionDTO>(qg.Question);
                await _questionService.EditQuestionAsync(qg.Question.Id, q);
            }

            return groupPublish;
        }
    }
}