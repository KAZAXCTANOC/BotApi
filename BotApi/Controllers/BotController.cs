using BotApi.Entities;
using BotApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : Controller
    {
        #region Base
        private readonly ILogger<BotController> _logger;
        public BotController(ILogger<BotController> logger)
        {
            _logger = logger;
        }

        [Route("Test")]
        public string TestMethod()
        {
            return "Test";
        } 
        #endregion

        [Route("SendRows")]
        public void SendRows()
        {
            var str = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8, true, 1024, true))
            {
                str = reader.ReadToEnd();
                Rows changes = JsonConvert.DeserializeObject<Rows>(str);

                if (changes.changes.activeSheet.ToString() != "ОП")
                {
                    return;
                }

                ChangeHelper changeHelper = new ChangeHelper(Convert.ToInt32(changes.changes.col), changes.needRow, changes.myUsersData );

                var placeholderType = changeHelper.DeterminingSourceChange();

                var isValid = changeHelper.Validate(placeholderType);
                if (isValid == true)
                {
                    var appointeeId = changes.myUsersData.FirstOrDefault(el => el.UserName.ToString() == changes.appointeeName.ToString()).TelegramId;
                    if (changes.changes.oldValue == null && changes.changes.newValue == null)
                    {
                        return;
                    }
                    //is'n change
                    if (changes.changes.oldValue == null && changes.changes.newValue != null)
                    {
                        switch (placeholderType)
                        {
                            case PlaceholderType.Applicant: //+
                                changeHelper.SendMessageForAppointeeAsync(changes.needRow, appointeeId);
                                break;

                            case PlaceholderType.Executor: //+
                                changeHelper.SendMessageForAppointeeAsync(changes.needRow, appointeeId, 1);
                                break;

                            case PlaceholderType.Assignee_Assignee: //+
                                changeHelper.SendMessageForExecutorAsync( changes.needRow);
                                break;

                            case PlaceholderType.Assignee_Acceptance: //+
                                changeHelper.SendMessageForApplicantAsync(changes.needRow);
                                break;

                            case PlaceholderType.Error:
                                break;
                        }
                    }
                    else
                    {
                        //is change
                        switch (placeholderType)
                        {
                            case PlaceholderType.Applicant:
                                changeHelper.SendCahngeMessageForAppointeeAsync(changes.needRow, changes.changes.oldValue.ToString(), Convert.ToInt32(changes.changes.col), appointeeId);
                                break;

                            case PlaceholderType.Executor:
                                changeHelper.SendMessageForAppointeeAsync(changes.needRow, appointeeId, 1);
                                break;

                            case PlaceholderType.Assignee_Assignee:
                                changeHelper.SendCahngeMessageForExecutorAsync(changes.needRow, changes.changes.oldValue.ToString());
                                break;

                            case PlaceholderType.Assignee_Acceptance:
                                changeHelper.SendMessageForApplicantAsync(changes.needRow);
                                break;

                            case PlaceholderType.Error:
                                break;
                        }
                    }
                }
            }
        }

    }
}
