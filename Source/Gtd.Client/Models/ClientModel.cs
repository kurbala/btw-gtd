﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Gtd.Client.Models
{
    public sealed class ClientModel
    {
        readonly IMessageQueue _queue;
        List<MutableInboxStuff> _thoughts = new List<MutableInboxStuff>();
        readonly Dictionary<StuffId, MutableInboxStuff> _thoughtDict = new Dictionary<StuffId, MutableInboxStuff>();

        readonly List<MutableProject> _projectList = new List<MutableProject>();
        readonly Dictionary<ProjectId, MutableProject> _projectDict = new Dictionary<ProjectId, MutableProject>();
        readonly Dictionary<ActionId, MutableAction> _actionDict = new Dictionary<ActionId, MutableAction>();
        


        static ImmutableProject Immute(MutableProject m)
        {
            var ma = m.Actions.Select(Immute).ToList().AsReadOnly();
            return new ImmutableProject(m.UIKey, m.ProjectId, m.Outcome, m.Type, ma);
        }

        static ImmutableAction Immute(MutableAction mutable)
        {
            return new ImmutableAction(mutable.UIKey,
                mutable.Id,
                mutable.Outcome,
                mutable.Completed,
                mutable.Archived,
                mutable.ProjectId,
                mutable.DeferUntil,
                mutable.DueDate);
        }



        public IList<ImmutableProject> ListProjects()
        {
            return _projectList.Select(Immute).ToList().AsReadOnly();
        } 

        public ImmutableProject GetProjectOrNull(ProjectId id)
        {
            MutableProject value;
            if (_projectDict.TryGetValue(id, out value))
                return Immute(value);
            return null;
        }

        public ImmutableInbox GetInbox()
        {
            return new ImmutableInbox(ImmutableList.Create(_thoughts.Select(f => f.Freeze()).ToArray()));
        }
        public int GetNumberOfThoughtsInInbox()
        {
            return _thoughts.Count;
        }

        public readonly TrustedSystemId Id;

        public ClientModel(IMessageQueue queue, TrustedSystemId id)
        {
            _queue = queue;
            Id = id;
        }

        bool _loadingCompleted;

        public void LoadingCompleted()
        {
            _loadingCompleted = true;

            var model = new ImmutableClientModel(
                ImmutableList.Create(_thoughts.Select(f => f.Freeze()).ToArray()),
                ImmutableList.Create(_projectList.Select(Immute).ToArray()));

            Publish(new Dumb.ClientModelLoaded(model));
        }

        void Publish(Dumb.CliendModelEvent e)
        {
            if (!_loadingCompleted)
                return;
            _queue.Enqueue(e);
        }

        public void ThoughtCaptured(StuffId stuffId, string thought, DateTime date)
        {
            var key = "thought-" + Id.Id;
            var item = new MutableInboxStuff(stuffId, thought, key);

            _thoughts.Add(item);
            _thoughtDict.Add(stuffId, item);
            Publish(new Dumb.InboxStuffAdded(item.Freeze(), _thoughts.Count));
        }

        public void ThoughtArchived(StuffId stuffId)
        {
            MutableInboxStuff value;
            if (!_thoughtDict.TryGetValue(stuffId,out value))
                return;

            _thoughts.Remove(value);

            Publish(new Dumb.InboxStuffRemoved(value.Freeze(), _thoughts.Count));
        }


        public void ProjectDefined(ProjectId projectId, string projectOutcome, ProjectType type)
        {
            var project = new MutableProject(projectId, projectOutcome, type);
            _projectList.Add(project);
            _projectDict.Add(projectId, project);
            

            Publish(new Dumb.ProjectAdded(project.UIKey, projectOutcome, projectId));
        }

        public void ActionDefined(ProjectId projectId, ActionId actionId, string outcome)
        {
            var action = new MutableAction(actionId, outcome, projectId);

            var project = _projectDict[projectId];
            project.Actions.Add(action);
            _actionDict.Add(actionId, action);

            Publish(new Dumb.ActionAdded(actionId, action.UIKey, projectId, project.UIKey, outcome));
        }
        public void ActionCompleted(ActionId actionId)
        {
            var action = _actionDict[actionId];
            var project = _projectDict[action.ProjectId];
            action.MarkAsCompleted();
            Publish(new Dumb.ActionUpdated(actionId, action.UIKey, action.ProjectId, project.UIKey, action.Outcome, true));
        }

        public void ThoughtSubjectChanged(StuffId stuffId, string subject)
        {
            _thoughtDict[stuffId].UpdateSubject(subject);
        }

        public void ProjectOutcomeChanged(ProjectId projectId, string outcome)
        {
            _projectDict[projectId].OutcomeChanged(outcome);
        }
        public void ActionOutcomeChanged(ActionId actionId, string outcome)
        {
            var action = _actionDict[actionId];
            var project = _projectDict[action.ProjectId];
            action.OutcomeChanged(outcome);
            Publish(new Dumb.ActionUpdated(actionId, action.UIKey, action.ProjectId, project.UIKey, action.Outcome, action.Completed));
        }

        public void ActionArchived(ActionId id)
        {
            _actionDict[id].MarkAsArchived();
        }

        public void ProjectTypeChanged(ProjectId projectId, ProjectType type)
        {
            _projectDict[projectId].TypeChanged(type);
        }

        public void DeferredUtil(ActionId actionId, DateTime deferUntil)
        {
            _actionDict[actionId].DeferUntilDate(deferUntil);
        }
        public void DueDateAssigned(ActionId actionId, DateTime newDueDate)
        {
            _actionDict[actionId].DueDateAssigned(newDueDate);
        }

        public void Verify(TrustedSystemId id)
        {
            if (Id.Id != id.Id)
                throw new InvalidOperationException();
        }

        public void Create(TrustedSystemId id)
        {
            
        }

        sealed class MutableInboxStuff
        {
            public StuffId StuffId { get; private set; }
            public string Subject { get; private set; }
            public  string UIKey { get; private set; }

            public MutableInboxStuff(StuffId stuffId, string subject, string uiKey)
            {
                StuffId = stuffId;
                Subject = subject;
                UIKey = uiKey;
            }


            public void UpdateSubject(string newSubject)
            {
                Subject = newSubject;
            }

            public ImmutableInboxStuff Freeze()
            {
                return new ImmutableInboxStuff(StuffId, Subject, UIKey);
            }
        }

         sealed class MutableProject 
        {
            public ProjectId ProjectId { get; private set; }
            public string Outcome { get; private set; }
            public ProjectType Type { get; private set; }
            public readonly string UIKey;

            public MutableProject(ProjectId projectId, string outcome, ProjectType type)
            {
                ProjectId = projectId;
                Outcome = outcome;
                Type = type;

                UIKey = "project-" + projectId.Id;
            }


            public readonly List<MutableAction> Actions = new List<MutableAction>();

            public void OutcomeChanged(string outcome)
            {
                Outcome = outcome;
            }

            public void TypeChanged(ProjectType type)
            {
                Type = type;
            }
        }

        sealed class MutableAction 
        {
            public ActionId Id { get; private set; }
            public string Outcome { get; private set; }
            public bool Completed { get; private set; }
            public bool Archived { get; private set; }
            public ProjectId ProjectId { get; private set; }
            public DateTime DeferUntil { get; private set; }
            public DateTime DueDate { get; private set; }

            public string UIKey { get { return "action-" + Id.Id; } }

            public MutableAction(ActionId action, string outcome, ProjectId project)
            {
                Id = action;
                Outcome = outcome;
                Completed = false;
                Archived = false;
                ProjectId = project;
            }

            public void MarkAsCompleted()
            {
                Completed = true;
            }
            public void OutcomeChanged(string outcome)
            {
                Outcome = outcome;
            }

            public void MarkAsArchived()
            {
                Archived = true;

            }

            public void DeferUntilDate(DateTime newStartDate)
            {
                DeferUntil = newStartDate;
            }
            public void DueDateAssigned(DateTime newDueDate)
            {
                DueDate = newDueDate;
            }
        }


    }

    

}