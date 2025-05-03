using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using ERE.DTO;
using ERE.Models;
using ERE.Repository;
using ERE.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERE.APIS;

public static class TeacherEndpoints
{
    public static void MapTeacherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/course", [Authorize] async (ITeacherRepository service, CreateCourseValidator validator, ClaimsPrincipal user, TeacherEndpoints.CreateCourseDto request) => {
            // Validate the request
            var identifier = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identifier)) {
                return Results.Unauthorized();
            }
            DTO.CreateCourseDto courseToCreate = new DTO.CreateCourseDto() {
                TeacherId = identifier,
                Level = request.Level,
                MaxScore = request.MaxScore,
                PassingRate = request.PassingRate,
                DayOfWeeks = request.DayOfWeeks,
                TimeOfDays = request.TimeOfDays
            };
            
            var result = new Response();
            try {
                validator.ValidateAndThrow(courseToCreate);
                result = await service.CreateCourse(courseToCreate);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });

        app.MapPost("/course-report", [Authorize] async (ITeacherRepository service, ClaimsPrincipal user, CreateCourseReportDto request) => {
                    // Validate the request
            var identifier = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identifier)) {
                return Results.Unauthorized();
            }    
            
            var result = new Response();
            try {
                // validator.ValidateAndThrow(courseToCreate);
                result = await service.CreateCourseReport(request);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });    
        // bulk mark course report as done or undone
        app.MapPut("/course-report", [Authorize] async (ITeacherRepository service, ClaimsPrincipal user, UpdateCourseReportStatusDto request) => {
                    // Validate the request
            var identifier = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identifier)) {
                return Results.Unauthorized();
            }    
            
            var result = new Response();
            try {
                // validator.ValidateAndThrow(courseToCreate);
                result = await service.UpdateCourseReportsStatus(request);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });    

        // app.MapPost("send-mail", (IMailService service,[FromBody] MailData email) => {
        //     try {
        //         // service.SendMail(email);
        //         var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        //         {
        //             Credentials = new NetworkCredential("ce2e673c8ab1a3", "0ac071d31fb48e"),
        //             EnableSsl = true
        //         };
        //         client.Send("vitouaj68@gmail.com", email.EmailToId, email.EmailSubject, email.EmailBody);
        //     } catch {
        //         throw;
        //     }
        // });
    }

    private class CreateCourseDto {
        public LevelId Level { get; set; }
        public float MaxScore { get; set; }
        public float PassingRate { get; set; }
        public List<Models.DayOfWeek> DayOfWeeks {get; set;}
        public List<TimeOfDay> TimeOfDays {get; set;}
    }
}