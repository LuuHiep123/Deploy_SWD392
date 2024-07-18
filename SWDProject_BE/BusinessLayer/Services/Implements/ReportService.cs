using AutoMapper;
using BusinessLayer.RequestModels.Report;
using BusinessLayer.ResponseModels.Product;
using BusinessLayer.ResponseModels.Report;
using DataLayer.Model;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implements
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<string> AddReportByUser(ReportRequestaUser dto, int userId)
        {
            try
            {
                var post = await _unitOfWork.Repository<Post>().GetById(dto.PostId);
                if (post == null)
                {
                    return "Post not found";
                }
                if (post.UserId == userId)
                {
                    return "You cannot report your own Post";
                }
                var result = _mapper.Map<Report>(dto);
                result.Status = false;
                result.UserId = userId;
                result.Date = DateTime.Now;
                await _unitOfWork.Repository<Report>().InsertAsync(result);
                await _unitOfWork.CommitAsync();
                return "Add successful!";


            }catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<string> DeleteReport(int id)
        {
            var report = await _unitOfWork.Repository<Report>().GetById(id);
            if(report != null)
            {
                await _unitOfWork.Repository<Report>().HardDelete(id);
                await _unitOfWork.CommitAsync();
                return "Delete successfull!";
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ReportResponseModel>> GetAll()
        {
            try
            {
                var Report = _unitOfWork.Repository<Report>().GetAll().ToList();
                List<ReportResponseModel> Final = new List<ReportResponseModel>();
                foreach (var report in Report)
                {
                    var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id.Equals(report.UserId));
                    var post = await _unitOfWork.Repository<Post>().FindAsync(c => c.Id.Equals(report.PostId));
                    ReportResponseModel result = new ReportResponseModel();
                    result = _mapper.Map<ReportResponseModel>(report);
                    result.UserName = user.UserName;
                    result.title = post.Title;
                    Final.Add(result);
                }
                return Final;

            }
            catch (Exception ex)
            {
                {
                    throw new Exception("Error DB!");
                }
            }
        }

        public async Task<List<ReportResponseModel>> GetAllValidReport()
        {
            try
            {
                var Report = _unitOfWork.Repository<Report>().FindAll(r => r.Status == true).ToList();
                List<ReportResponseModel> Final = new List<ReportResponseModel>();
                foreach (var report in Report)
                {
                    var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id.Equals(report.UserId));
                    var post = await _unitOfWork.Repository<Post>().FindAsync(c => c.Id.Equals(report.PostId));
                    ReportResponseModel result = new ReportResponseModel();
                    result = _mapper.Map<ReportResponseModel>(report);
                    result.UserName = user.UserName;
                    result.title = post.Title;
                    Final.Add(result);
                }
                return Final;

            }
            catch (Exception ex)
            {
                {
                    throw new Exception("Error DB!");
                }
            }
        }

        public async Task<List<ReportResponseModel>> GetReportByPostId(int postId)
        {
            try
            {
                var findPost = await _unitOfWork.Repository<Post>().GetById(postId);
                if (findPost != null)
                {
                    var Report = _unitOfWork.Repository<Report>().FindAll(r => r.PostId == postId).ToList();
                    List<ReportResponseModel> Final = new List<ReportResponseModel>();
                    foreach (var report in Report)
                    {
                        var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id.Equals(report.UserId));
                        var post = await _unitOfWork.Repository<Post>().FindAsync(c => c.Id.Equals(report.PostId));
                        ReportResponseModel result = new ReportResponseModel();
                        result = _mapper.Map<ReportResponseModel>(report);
                        result.UserName = user.UserName;
                        result.title = post.Title;
                        Final.Add(result);
                    }
                    return Final;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                {
                    throw new Exception("Error DB!");
                }
            }
        }

        public async Task<List<ReportResponseModel>> GetReportByUserId(int userId)
        {
            try
            {
                var findUser = await _unitOfWork.Repository<User>().GetById(userId);
                if(findUser != null)
                {
                    var Report = _unitOfWork.Repository<Report>().FindAll(r => r.UserId == userId).ToList();
                    List<ReportResponseModel> Final = new List<ReportResponseModel>();
                    foreach (var report in Report)
                    {
                        var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id.Equals(report.UserId));
                        var post = await _unitOfWork.Repository<Post>().FindAsync(c => c.Id.Equals(report.PostId));
                        ReportResponseModel result = new ReportResponseModel();
                        result = _mapper.Map<ReportResponseModel>(report);
                        result.UserName = user.UserName;
                        result.title = post.Title;
                        Final.Add(result);
                    }
                    return Final;
                }
                else
                {
                    return null;
                }
                

            }
            catch (Exception ex)
            {
                {
                    throw new Exception("Error DB!");
                }
            }
        }

        public async Task<string> UpdateReportByUser(int id, ReportRequestaUser dto)
        {
            var report = await _unitOfWork.Repository<Report>().GetById(id);
            if(report != null)
            {
                if(dto.PostId != 0)
                {
                    report.PostId = dto.PostId;
                }
                if (!dto.Description.IsNullOrEmpty())
                {
                    report.Description = dto.Description;
                }
                await _unitOfWork.Repository<Report>().Update(report,id);   
                await _unitOfWork.CommitAsync();
                return "Update Successful!";
            }
            return null;
        }

        public async Task<string> AcceptReport(int id)
        {
            var report = await _unitOfWork.Repository<Report>().GetById(id);
            if (report != null)
            {
                report.Status = true;
                await _unitOfWork.Repository<Report>().Update(report, id);
                await _unitOfWork.CommitAsync();

                // Get all other reports related to the same post with Status = false
                var relatedReports =  _unitOfWork.Repository<Report>()
                    .FindAll(r => r.PostId == report.PostId && r.Status == false);

                // Delete those reports
                foreach (var relatedReport in relatedReports)
                {
                    await _unitOfWork.Repository<Report>().HardDelete(relatedReport.Id);
                    await _unitOfWork.CommitAsync();
                }

                // Set the post's PublicStatus to false
                var post = await _unitOfWork.Repository<Post>().GetById(report.PostId);
                if (post != null)
                {
                    post.PublicStatus = false;
                    await _unitOfWork.Repository<Post>().Update(post, post.Id);
                    await _unitOfWork.CommitAsync();

                    // Get all Exchangeds related to the post with ExchangedStatus = false
                    var relatedExchangeds = await _unitOfWork.Repository<Exchanged>()
                        .GetAll().Where(e => e.PostId == post.Id && e.Status == false).ToListAsync();

                    // Delete those Exchangeds and their related ExchangedProducts
                    foreach (var exchanged in relatedExchangeds)
                    {
                        var exchangedProducts = await _unitOfWork.Repository<ExchangedProduct>()
                            .GetAll().Where(ep => ep.ExchangeId == exchanged.Id).ToListAsync();

                        foreach (var exchangedProduct in exchangedProducts)
                        {
                            // Update the product use in exchange status to true
                            var product = await _unitOfWork.Repository<Product>().GetById(exchangedProduct.ProductId);
                            if (product != null)
                            {
                                product.Status = true;
                                await _unitOfWork.Repository<Product>().Update(product, product.Id);
                            }

                            await _unitOfWork.Repository<ExchangedProduct>().HardDelete(exchangedProduct.Id);
                            await _unitOfWork.CommitAsync();
                        }

                        await _unitOfWork.Repository<Exchanged>().HardDelete(exchanged.Id);
                        await _unitOfWork.CommitAsync();
                    }
                }
                
                return "Accept Successful!";
            }
            return null;
        }
    }
}
