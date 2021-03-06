﻿using System.Collections.Generic;
using System.Linq;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace Orchard.Workflows.Activities {
    public class RedirectActivity : Task {
        private readonly IWorkContextAccessor _wca;

        public RedirectActivity(IWorkContextAccessor wca) {
            _wca = wca;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext) {
            return true;
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            return Enumerable.Empty<LocalizedString>();
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var url = activityContext.GetState<string>("Url");
            _wca.GetContext().HttpContext.Response.Redirect(url);
            return Enumerable.Empty<LocalizedString>();
        }

        public override string Name {
            get { return "Redirect"; }
        }

        public override LocalizedString Category {
            get { return T("HTTP"); }
        }

        public override LocalizedString Description {
            get { return T("Redirect to the specified URL."); }
        }

        public override string Form {
            get { return "ActionRedirect"; }
        }
    }
}