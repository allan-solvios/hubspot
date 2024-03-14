using Cannolai.Hubspot.Entity;
using Cannolai.Hubspot.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Cannolai.Hubspot
{
    public class HubspotUtility
    {
        public Response _response;

        public HubspotUtility()
        {
            _response = new Response();
        }

        public async Task<Response> AddContactColumn(ContactColumn contactColumn, List<string> accessTokens)
        {
            try
            {
                foreach (var token in accessTokens)
                {
                    string groupName = contactColumn?.groupName?.Trim();
                    var httpClientService = new HttpClientService();
                    var getGroupUrl = $"https://api.hubapi.com/properties/v1/contacts/groups/named/{groupName.ToLower().Replace(" ", "_")}";
                    var getGroupResult = await httpClientService.GetAsync(getGroupUrl, token);
                    if (!getGroupResult.IsSuccess)
                    {
                        var addGroupUrl = $"https://api.hubapi.com/properties/v1/contacts/groups";
                        var newGroup = new Group()
                        {
                            name = groupName.ToLower().Replace(" ", "_"),
                            displayName = groupName,
                        };
                        var addGroupResult = await httpClientService.PostAsync(addGroupUrl, token, newGroup);
                        if (!addGroupResult.IsSuccess)
                        {
                            _response?.ResponseModel?.Add(addGroupResult);
                            break;
                        }
                    }

                    contactColumn.name = contactColumn.name?.ToLower()?.Trim().Replace(" ", "_");
                    contactColumn.label = contactColumn.label?.Trim();
                    contactColumn.description = contactColumn.description?.Trim();
                    contactColumn.groupName = groupName;

                    var addContactPropertyUrl = "https://api.hubapi.com/crm/v3/properties/contacts";
                    var addColumnResult = await httpClientService.PostAsync(addContactPropertyUrl, token, contactColumn);
                    _response?.ResponseModel?.Add(addColumnResult);
                }
                return _response;
            }
            catch (Exception ex)
            {
                return _response;
            }
        }

        public async Task<Response> AddCompanyColumn(CompanyColumn companyColumn, List<string> accessTokens)
        {
            try
            {
                foreach (var token in accessTokens)
                {
                    string groupName = companyColumn?.groupName?.Trim();
                    var httpClientService = new HttpClientService();
                    var getGroupUrl = $"https://api.hubapi.com/properties/v1/companies/groups/named/{groupName.ToLower().Replace(" ", "_")}";
                    var getGroupResult = await httpClientService.GetAsync(getGroupUrl, token);
                    if (!getGroupResult.IsSuccess)
                    {
                        var addGroupUrl = $"https://api.hubapi.com/properties/v1/companies/groups";
                        var newGroup = new Group()
                        {
                            name = groupName.ToLower().Replace(" ", "_"),
                            displayName = groupName,
                        };
                        var addGroupResult = await httpClientService.PostAsync(addGroupUrl, token, newGroup);
                        if (!addGroupResult.IsSuccess)
                        {
                            _response?.ResponseModel?.Add(addGroupResult);
                            break;
                        }
                    }

                    companyColumn.name = companyColumn.name?.ToLower()?.Trim().Replace(" ", "_");
                    companyColumn.label = companyColumn.label?.Trim();
                    companyColumn.description = companyColumn.description?.Trim();
                    companyColumn.groupName = groupName;

                    var addCompanyPropertyUrl = "https://api.hubapi.com/properties/v1/companies/properties";
                    var addColumnResult = await httpClientService.PostAsync(addCompanyPropertyUrl, token, companyColumn);
                    _response?.ResponseModel?.Add(addColumnResult);
                }
                return _response;
            }
            catch (Exception ex)
            {
                return _response;
            }
        }
    }
}
