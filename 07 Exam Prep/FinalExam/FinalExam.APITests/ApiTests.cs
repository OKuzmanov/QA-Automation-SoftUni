using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace FinalExam.APITests
{
    public class ApiTests
    {
        private const string url = "https://taskboard.nakov.repl.co";
        private RestClient client;

        [OneTimeSetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_ListAllTasks_AssertFirstTaskTitle()
        {
            const string firstTaskTitle = "Project skeleton";
            const string board = "Done";

            var request = new RestRequest(url + "/api/tasks", Method.Get);

            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var requestTasksDoneBoard = new RestRequest(url + "/api/tasks/board/" + board, Method.Get);

            var responseTasksDoneBoard = this.client.Execute(requestTasksDoneBoard);

            List<Task>? DoneBoardtasks = JsonSerializer.Deserialize<List<Task>>(responseTasksDoneBoard.Content);

            Assert.That(responseTasksDoneBoard.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(DoneBoardtasks[0].title, Is.EqualTo(firstTaskTitle));
        }

        [Test]
        public void Test_GetTaskByValidKeyword_Home()
        {
            const string keyword = "home";
            const string firstTaskTitle = "Home page";

            var request = new RestRequest(url + "/api/tasks/search/" + keyword, Method.Get);

            var response = this.client.Execute(request);

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);

            Task firstResult = tasks[0];

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(firstResult.title, Is.EqualTo(firstTaskTitle));
        }

        [Test]
        public void Test_GetTaskByInvalidKeyword()
        {
            string keyword = "missing" + DateTime.Now.Ticks;

            var request = new RestRequest(url + "/api/tasks/search/" + keyword, Method.Get);

            var response = this.client.Execute(request);

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_CreateTask_InvalidData()
        {
            const string errMsg = "{\"errMsg\":\"Title cannot be empty!\"}";

            var request = new RestRequest(url + "/api/tasks", Method.Post);

            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(errMsg, Is.EqualTo(response.Content));
        }

        [Test]
        public void Test_CreateTask_ValidData()
        {
            //Get Count Tasks Before Add
            var getAllTasksRequest = new RestRequest(url + "/api/tasks", Method.Get);

            var AllTasksResponseBefore = this.client.Execute(getAllTasksRequest);

            var countTasksBefore = JsonSerializer.Deserialize<List<Task>>(AllTasksResponseBefore.Content).Count;

            //Create New Task
            string newTitle = "Add Tests" + DateTime.Now.Ticks;
            string newDescription = "API + UI tests" + DateTime.Now.Ticks;
            string board = "Open";

            var request = new RestRequest(url + "/api/tasks", Method.Post);
            request.AddJsonBody(new {title = newTitle, description = newDescription, board = board});

            var response = this.client.Execute(request);

            Assert.That(HttpStatusCode.Created, Is.EqualTo(response.StatusCode));

            //Get All Tasks After
            var AllTasksResponseAfter = this.client.Execute(getAllTasksRequest);

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(AllTasksResponseAfter.Content);

            Task lastTask = tasks.Last();

            Assert.That(lastTask.title, Is.EqualTo(newTitle));
            Assert.That(lastTask.description, Is.EqualTo(newDescription));
            Assert.That(lastTask.board.name, Is.EqualTo(board));
            Assert.That(countTasksBefore < tasks.Count);
        }
    }
}