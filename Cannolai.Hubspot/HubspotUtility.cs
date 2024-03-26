using Cannolai.Hubspot.Entity;
using Cannolai.Hubspot.Utility;
using System.Net;
using System.Text.Json;
using System.Xml;

namespace Cannolai.Hubspot
{
    public class HubspotUtility
    {
        public Response _response;
        public HttpClientService httpClientService;

        public HubspotUtility()
        {
            _response = new Response();
            httpClientService  = new HttpClientService();
        }

        #region Contact methods

        public async Task<Response> AddContactColumnAsync(ContactColumn contactColumn, List<string> accessTokens)
        {
            try
            {
                foreach (var token in accessTokens)
                {
                    string groupName = contactColumn?.groupName?.Trim();
                    //var httpClientService = new HttpClientService();
                    var getGroupUrl = $"https://api.hubapi.com/properties/v1/contacts/groups/named/{groupName.ToLower().Replace(" ", "_")}";
                    var (getGroupResult, getGrpStatusCode) = await httpClientService.GetAsync(getGroupUrl, token);
                    if (getGrpStatusCode != null
                        && getGrpStatusCode != HttpStatusCode.Unauthorized)
                    {
                        if (!getGroupResult.IsSuccess)
                        {
                            var addGroupUrl = $"https://api.hubapi.com/properties/v1/contacts/groups";
                            var newGroup = new Group()
                            {
                                name = groupName.ToLower().Replace(" ", "_"),
                                displayName = groupName,
                            };
                            var (addGroupResult, addGrpStatusCode) = await httpClientService.PostAsync(addGroupUrl, token, newGroup);
                            if (!addGroupResult.IsSuccess && addGrpStatusCode != null
                                && addGrpStatusCode != HttpStatusCode.Unauthorized)
                            {
                                _response?.ResponseModel?.Add(addGroupResult);
                                break;
                            }
                        }
                    }
                    else
                    {
                        _response?.ResponseModel?.Add(getGroupResult);
                        return _response;
                    }

                    contactColumn.name = contactColumn.name?.ToLower()?.Trim().Replace(" ", "_");
                    contactColumn.label = contactColumn.label?.Trim();
                    contactColumn.description = contactColumn.description?.Trim();
                    contactColumn.groupName = groupName;

                    var addContactPropertyUrl = "https://api.hubapi.com/crm/v3/properties/contacts";
                    var (addColumnResult, addColumnResultStatusCode) = await httpClientService.PostAsync(addContactPropertyUrl, token, contactColumn);
                    _response?.ResponseModel?.Add(addColumnResult);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response?.ResponseModel?.Add(new ResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex?.InnerException?.ToString()
                });
                return _response;
            }
        }

        public async Task<Response?> GetContactByIdAsync(string hsContactId, string token)
        {
            try
            {
                var getContactUrl = $"https://api.hubapi.com/crm/v3/objects/contacts/{hsContactId}";
                var (getContactResult, getContactStatusCode) = await httpClientService.GetAsync(getContactUrl, token);

                _response?.ResponseModel?.Add(getContactResult);
                return _response;
            }
            catch (Exception ex)
            {
                _response?.ResponseModel?.Add(new ResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex?.InnerException?.ToString()
                });
                return _response;
            }
        }

        public async Task<Response?> SaveContactAsync(ContactDTO contact, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(contact.HsContactId))
                {
                    var propertiesObj = new
                    {
                        properties = contact?.Properties?.ToDictionary(p => p.Name, p => p.Value)
                    };
                    var updateContactUrl = $"https://api.hubapi.com/crm/v3/objects/contacts/{contact?.HsContactId}";
                    var (updateContactResult, updateContactStatusCode) = await httpClientService.PatchAsync(updateContactUrl, token, propertiesObj);
                    _response?.ResponseModel?.Add(updateContactResult);
                    return _response;
                }
                else
                {
                    var propertiesObj = new
                    {
                        properties = contact?.Properties?.ToDictionary(p => p.Name, p => p.Value)
                    };
                    var addContactUrl = $"https://api.hubapi.com/crm/v3/objects/contacts";
                    var (addContactResult, addContactStatusCode) = await httpClientService.PostAsync(addContactUrl, token, propertiesObj);
                    _response?.ResponseModel?.Add(addContactResult);
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response?.ResponseModel?.Add(new ResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex?.InnerException?.ToString()
                });
                return _response;
            }
        }
        #endregion

        #region Company methods
        public async Task<Response> AddCompanyColumnAsync(CompanyColumn companyColumn, List<string> accessTokens)
        {
            try
            {
                foreach (var token in accessTokens)
                {
                    string groupName = companyColumn?.groupName?.Trim();
                    var httpClientService = new HttpClientService();
                    var getGroupUrl = $"https://api.hubapi.com/properties/v1/companies/groups/named/{groupName.ToLower().Replace(" ", "_")}";
                    var (getGroupResult, getGrpResulStatusCode) = await httpClientService.GetAsync(getGroupUrl, token);

                    if (getGrpResulStatusCode != null && getGrpResulStatusCode != HttpStatusCode.Unauthorized)
                    {
                        if (!getGroupResult.IsSuccess)
                        {
                            var addGroupUrl = $"https://api.hubapi.com/properties/v1/companies/groups";
                            var newGroup = new Group()
                            {
                                name = groupName.ToLower().Replace(" ", "_"),
                                displayName = groupName,
                            };
                            var (addGroupResult, addGrpStatusCode) = await httpClientService.PostAsync(addGroupUrl, token, newGroup);
                            if (!addGroupResult.IsSuccess)
                            {
                                _response?.ResponseModel?.Add(addGroupResult);
                                break;
                            }
                        }
                    }
                    else
                    {
                        _response?.ResponseModel?.Add(getGroupResult);
                        return _response;
                    }

                    companyColumn.name = companyColumn.name?.ToLower()?.Trim().Replace(" ", "_");
                    companyColumn.label = companyColumn.label?.Trim();
                    companyColumn.description = companyColumn.description?.Trim();
                    companyColumn.groupName = groupName;

                    var addCompanyPropertyUrl = "https://api.hubapi.com/properties/v1/companies/properties";
                    var (addColumnResult, addColumnResultStatusCode) = await httpClientService.PostAsync(addCompanyPropertyUrl, token, companyColumn);
                    _response?.ResponseModel?.Add(addColumnResult);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response?.ResponseModel?.Add(new ResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex?.InnerException?.ToString()
                });
                return _response;
            }
        }

        public async Task<Response?> GetCompanyByIdAsync(string hsCompanyId, string token)
        {
            try
            {
                var getCompanyUrl = $"https://api.hubapi.com/crm/v3/objects/companies/{hsCompanyId}";
                var (getCompanyResult, getCompanyStatusCode) = await httpClientService.GetAsync(getCompanyUrl, token);

                _response?.ResponseModel?.Add(getCompanyResult);
                return _response;
            }
            catch (Exception ex)
            {
                _response?.ResponseModel?.Add(new ResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex?.InnerException?.ToString()
                });
                return _response;
            }
        }

        public async Task<Response?> SaveCompanyAsync(CompanyDTO company, string token)
        {
            try
            {
                if(!string.IsNullOrEmpty(company.HsCompanyId))
                {
                    var propertiesObj = new
                    {
                        properties = company?.Properties?.ToDictionary(p => p.Name, p => p.Value)
                    };
                    var updateCompanyUrl = $"https://api.hubapi.com/crm/v3/objects/companies/{company?.HsCompanyId}";
                    var (updateCompanyResult, updateCompanyStatusCode) = await httpClientService.PatchAsync(updateCompanyUrl, token, propertiesObj);
                    _response?.ResponseModel?.Add(updateCompanyResult);
                    return _response;
                }
                else
                {
                    var propertiesObj = new
                    {
                        properties = company?.Properties?
                        .ToDictionary(p => p.Name, p => p.Value)
                    };
                    var addCompanyUrl = $"https://api.hubapi.com/crm/v3/objects/companies";
                    var (addCompanyResult, addCompanyStatusCode) = await httpClientService.PostAsync(addCompanyUrl, token, propertiesObj);
                    _response?.ResponseModel?.Add(addCompanyResult);
                    return _response;
                }
            }
            catch(Exception ex) 
            {
                _response?.ResponseModel?.Add(new ResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex?.InnerException?.ToString()
                });
                return _response;
            }
        }
        #endregion
    }
}
