using AutoMapper;
using BusinessLayer.RequestModels.Appeal;
using BusinessLayer.ResponseModels.Appeal;
using DataLayer.Dto.Product;
using DataLayer.Model;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implements
{
    public class AppealService : IAppealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBannedAccountService _bannedAccountService;
        private IMapper _mapper;

        public AppealService(IUnitOfWork unitOfWork, IMapper mapper, IBannedAccountService bannedAccountService )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bannedAccountService = bannedAccountService;
        }

        public async Task<string> AcceptAppeal(int AppealId)
        {
            try
            {
                var appeal = _unitOfWork.Repository<Appeal>().Find(a => a.Id == AppealId).FirstOrDefault();
                if(appeal != null)
                {
                    appeal.ModifiedDate = DateTime.Now;
                    appeal.Status = true;
                    await _unitOfWork.Repository<Appeal>().Update(appeal, AppealId);
                    await _bannedAccountService.UnBanUser(appeal.UserId);          
                    return "Unban Sucessful!";
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> AddAppeal(AddAppealRequestModel dto, int Userid)
        {
            try
            {
                var result = _mapper.Map<Appeal>(dto);
                result.Status = false;
                result.UserId = Userid;
                result.Date = DateTime.Now;
                await _unitOfWork.Repository<Appeal>().InsertAsync(result);
                await _unitOfWork.CommitAsync();
                return "Add successfull!";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AppealResponseModel> FindAppealById(int AppealId)
        {
            try
            {
                var appeal = await _unitOfWork.Repository<Appeal>().FindAsync(a => a.Id == AppealId);           
                if(appeal != null)
                {             
                    var result = _mapper.Map<AppealResponseModel>(appeal);
                    var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id == appeal.UserId);
                    if (user == null)
                    {
                        result.UserName = "not found";
                    }
                    else
                    {
                        result.UserName = user.UserName;
                    }

                    var bannerUser = await _unitOfWork.Repository<BannedAccount>().FindAsync(b => b.Id == appeal.BannerAcountId);
                    if (bannerUser == null)
                    {
                        result.BannerDescription = "not found";
                    }
                    else
                    {
                        result.BannerDescription = bannerUser.Description;
                        result.BannerDate = bannerUser.Date;
                    }
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AppealResponseModel>> GetAll()
        {
            try
            {
                var appealList = await _unitOfWork.Repository<Appeal>().GetAll().ToListAsync();
                List<AppealResponseModel> listAppealModel = new List<AppealResponseModel>();
                foreach (var appeal in appealList)
                {
                    AppealResponseModel appealModel = new AppealResponseModel();
                    appealModel = _mapper.Map<AppealResponseModel>(appeal);

                    var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id == appeal.UserId);
                    if (user == null)
                    {
                        appealModel.UserName = "not found";
                    }
                    else
                    {
                        appealModel.UserName = user.UserName;
                    }
                    var bannerUser = await _unitOfWork.Repository<BannedAccount>().FindAsync(b => b.Id == appeal.BannerAcountId);
                    if (bannerUser == null)
                    {
                        appealModel.BannerDescription = "not found";
                    }
                    else
                    {
                        appealModel.BannerDescription = bannerUser.Description;
                        appealModel.BannerDate = bannerUser.Date;
                    }
                    listAppealModel.Add(appealModel);
                }
                return listAppealModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AppealResponseModel>> GetAllAppealProcessingll()
        {
            try
            {
                var appealList =  _unitOfWork.Repository<Appeal>().FindAll(a => a.Status == false).ToList();
                List<AppealResponseModel> listAppealModel = new List<AppealResponseModel>();
                foreach (var appeal in appealList)
                {
                    AppealResponseModel appealModel = new AppealResponseModel();
                    appealModel = _mapper.Map<AppealResponseModel>(appeal);

                    var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id == appeal.UserId);
                    if (user == null)
                    {
                        appealModel.UserName = "not found";
                    }
                    else
                    {
                        appealModel.UserName = user.UserName;
                    }
                    var bannerUser = await _unitOfWork.Repository<BannedAccount>().FindAsync(b => b.Id == appeal.BannerAcountId);
                    if (bannerUser == null)
                    {
                        appealModel.BannerDescription = "not found";
                    }
                    else
                    {
                        appealModel.BannerDescription = bannerUser.Description;
                        appealModel.BannerDate = bannerUser.Date;
                    }
                    listAppealModel.Add(appealModel);
                }
                return listAppealModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AppealResponseModel>> GetAllByBannerAccountId(int BannerAccountId)
        {
            try
            {
                var checkBanner = _unitOfWork.Repository<BannedAccount>().Find(b => b.Id == BannerAccountId).FirstOrDefault();
                if (checkBanner == null)
                {
                    return null;
                }
                var appealList = _unitOfWork.Repository<Appeal>().FindAll(a => a.BannerAcountId == BannerAccountId).ToList();
                List<AppealResponseModel> listAppealModel = new List<AppealResponseModel>();
                foreach (var appeal in appealList)
                {
                    AppealResponseModel appealModel = new AppealResponseModel();
                    appealModel = _mapper.Map<AppealResponseModel>(appeal);

                    var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id == appeal.UserId);
                    if (user == null)
                    {
                        appealModel.UserName = "not found";
                    }
                    else
                    {
                        appealModel.UserName = user.UserName;
                    }
                    var bannerUser = await _unitOfWork.Repository<BannedAccount>().FindAsync(b => b.Id == appeal.BannerAcountId);
                    if (bannerUser == null)
                    {
                        appealModel.BannerDescription = "not found";
                    }
                    else
                    {
                        appealModel.BannerDescription = bannerUser.Description;
                        appealModel.BannerDate = bannerUser.Date;
                    }
                    listAppealModel.Add(appealModel);
                }
                return listAppealModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AppealResponseModel>> GetAllByUserId(int userId)
        {
            try
            {
                var checkUser = _unitOfWork.Repository<User>().Find(u => u.Id == userId).FirstOrDefault();
                if (checkUser == null) 
                {
                    return null;
                }
                var appealList =  _unitOfWork.Repository<Appeal>().FindAll(a => a.UserId == userId).ToList();
                List<AppealResponseModel> listAppealModel = new List<AppealResponseModel>();
                foreach (var appeal in appealList)
                {
                    AppealResponseModel appealModel = new AppealResponseModel();
                    appealModel = _mapper.Map<AppealResponseModel>(appeal);

                    var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id == appeal.UserId);
                    if (user == null)
                    {
                        appealModel.UserName = "not found";
                    }
                    else
                    {
                        appealModel.UserName = user.UserName;
                    }
                    var bannerUser = await _unitOfWork.Repository<BannedAccount>().FindAsync(b => b.Id == appeal.BannerAcountId);
                    if (bannerUser == null)
                    {
                        appealModel.BannerDescription = "not found";
                    }
                    else
                    {
                        appealModel.BannerDescription = bannerUser.Description;
                        appealModel.BannerDate = bannerUser.Date;
                    }
                    listAppealModel.Add(appealModel);
                }
                return listAppealModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateAppeal(UppdateAppealRequestModel dto, int appealId)
        {
            try
            {
                var appeal = await _unitOfWork.Repository<Appeal>().FindAsync(a => a.Id == appealId);
                if (appeal != null)
                {
                    appeal.Description = dto.Description;
                    appeal.ModifiedDate = DateTime.Now;
                    await _unitOfWork.CommitAsync();
                    return "Update Successfull!";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
