using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace TaskBoard.APITEsts
{
    public class ApiTests
    {
        const string baseURl = "https://taskboard.nakov.repl.co";
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_ListAllTasks()
        {
            RestRequest request = new RestRequest(baseURl + "/api/tasks", Method.Get);

            RestResponse response = this.client.Execute(request);

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
                
            Task lastTask = tasks.Last();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count > 0);
            Assert.AreNotEqual(lastTask.id, null);
            Assert.AreNotEqual(lastTask.title, null);
            Assert.AreNotEqual(lastTask.board, null);
        }

        [Test]
        public void Test_ListAllOpenBoardTasks()
        {
            string board = "Open";

            RestRequest request = new RestRequest(baseURl + "/api/tasks/board/" + board, Method.Get);

            RestResponse response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);


            foreach (Task task in tasks)
            {
                Assert.That(task.board.name, Is.EqualTo(board));
            }
        }

        [Test]
        public void Test_ListAllInProgressBoardTasks()
        {
            string board = "In Progress";

            RestRequest request = new RestRequest(baseURl + "/api/tasks/board/" + board, Method.Get);

            RestResponse response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);


            foreach (Task task in tasks)
            {
                Assert.That(task.board.name, Is.EqualTo(board));
            }
        }

        [Test]
        public void Test_ListAllDoneBoardTasks()
        {
            string board = "Done";

            RestRequest request = new RestRequest(baseURl + "/api/tasks/board/" + board, Method.Get);

            RestResponse response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);


            foreach (Task task in tasks)
            {
                Assert.That(task.board.name, Is.EqualTo(board));
            }
        }

        [Test]
        public void Test_CreateTasksOpenBoard_ValidInput()
        {
            string newTitle = "New Title " + DateTime.Now.Ticks;
            string newDescription = "New Description " + DateTime.Now.Ticks;
            string board = "Open";

            //Create new task
            RestRequest request = new RestRequest(baseURl + "/api/tasks", Method.Post);
            request.AddJsonBody(new {title = newTitle, description = newDescription, board = board});

            RestResponse response = this.client.Execute(request);

            //Get all tasks
            RestRequest requestAll = new RestRequest(baseURl + "/api/tasks/board/" + board, Method.Get);

            RestResponse responseAll = this.client.Execute(requestAll);

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(responseAll.Content);

            Task lastTask = tasks.Last();

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.AreEqual(newTitle, lastTask.title);
            Assert.AreEqual(newDescription, lastTask.description);
        }

        [Test]
        public void Test_CreateTasksInProgressBoard_ValidInput()
        {
            string newTitle = "New Title " + DateTime.Now.Ticks;
            string newDescription = "New Description " + DateTime.Now.Ticks;
            string board = "In Progress";

            //Create new task
            RestRequest request = new RestRequest(baseURl + "/api/tasks", Method.Post);
            request.AddJsonBody(new { title = newTitle, description = newDescription, board = board });

            RestResponse response = this.client.Execute(request);

            //Get all tasks
            RestRequest requestAll = new RestRequest(baseURl + "/api/tasks/board/" + board, Method.Get);

            RestResponse responseAll = this.client.Execute(requestAll);

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(responseAll.Content);

            Task lastTask = tasks.Last();

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.AreEqual(newTitle, lastTask.title);
            Assert.AreEqual(newDescription, lastTask.description);
        }

        [Test]
        public void Test_CreateTasksDoneBoard_ValidInput()
        {
            string newTitle = "New Title " + DateTime.Now.Ticks;
            string newDescription = "New Description " + DateTime.Now.Ticks;
            string board = "Done";

            //Create new task
            RestRequest request = new RestRequest(baseURl + "/api/tasks", Method.Post);
            request.AddJsonBody(new { title = newTitle, description = newDescription, board = board });

            RestResponse response = this.client.Execute(request);

            //Get all tasks
            RestRequest requestAll = new RestRequest(baseURl + "/api/tasks/board/" + board, Method.Get);

            RestResponse responseAll = this.client.Execute(requestAll);

            List<Task>? tasks = JsonSerializer.Deserialize<List<Task>>(responseAll.Content);

            Task lastTask = tasks.Last();

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.AreEqual(newTitle, lastTask.title);
            Assert.AreEqual(newDescription, lastTask.description);
        }

        [Test]
        public void Test_CreateTasksBoard_InvalidInput()
        {
            const string errMsg = "{\"errMsg\":\"Title cannot be empty!\"}";
            string newDescription = "New Description " + DateTime.Now.Ticks;
            string board = "Open";

            RestRequest request = new RestRequest(baseURl + "/api/tasks", Method.Post);
            request.AddJsonBody(new { description = newDescription, board = board });

            RestResponse response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.AreEqual(errMsg, response.Content);

        }

        [Test]
        public void Test_SearchByKeyword_ValidInput()
        {
            string newTitle = "New Title " + DateTime.Now.Ticks;
            string newDescription = "New Description " + DateTime.Now.Ticks;
            string board = "Open";

            //Create new task
            RestRequest request = new RestRequest(baseURl + "/api/tasks", Method.Post);
            request.AddJsonBody(new { title = newTitle, description = newDescription, board = board });

            RestResponse response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            RestRequest searchRequest = new RestRequest(baseURl + "/api/tasks/search/" + newTitle);

            RestResponse searchResponse = this.client.Execute(searchRequest);

            var searchResults = JsonSerializer.Deserialize<List<Task>>(searchResponse.Content);
            Task firstTaskResult = searchResults[0];

            //Assert
            Assert.AreEqual(searchResponse.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(newTitle, firstTaskResult.title);
            Assert.AreEqual(newDescription, firstTaskResult.description);
        }

        [Test]
        public void Test_SearchByKeyword_InvalidInput()
        {
            string keyword = "Invalid Rand " + DateTime.Now.Ticks;

            RestRequest searchRequest = new RestRequest(baseURl + "/api/tasks/search/" + keyword);

            RestResponse searchResponse = this.client.Execute(searchRequest);

            var searchResults = JsonSerializer.Deserialize<List<Task>>(searchResponse.Content);

            //Assert
            Assert.AreEqual(searchResponse.StatusCode, HttpStatusCode.OK);
            Assert.That(searchResults.Count == 0);
        }

        [Test]
        public void Test_DeleteTask_ValidId()
        {
            string newTitle = "New Title " + DateTime.Now.Ticks;
            string newDescription = "New Description " + DateTime.Now.Ticks;
            string board = "Open";

            //Create new task
            RestRequest request = new RestRequest(baseURl + "/api/tasks", Method.Post);
            request.AddJsonBody(new { title = newTitle, description = newDescription, board = board });

            RestResponse response = this.client.Execute(request);

            string json = response.Content.Substring(28);
            json = json.Remove(json.Length - 1, 1);
            Task? deletedTask = JsonSerializer.Deserialize<Task>(json);

            //Get Count Tasks Before Delete
            RestRequest requestAllBefore = new RestRequest(baseURl + "/api/tasks", Method.Get);

            RestResponse responseAllBefore = this.client.Execute(requestAllBefore);

            var tasksCountBefore = JsonSerializer.Deserialize<List<Task>>(responseAllBefore.Content).Count;

            //Delete Task
            RestRequest requestDelete = new RestRequest(baseURl + "/api/tasks/" + deletedTask.id, Method.Delete);

            RestResponse responseDelete = this.client.Execute(requestDelete);

            Assert.AreEqual(HttpStatusCode.OK, responseDelete.StatusCode);

            //Get Count Tasks After Delete
            RestRequest requestAllAfter = new RestRequest(baseURl + "/api/tasks", Method.Get);

            RestResponse responseAllAfter = this.client.Execute(requestAllAfter);

            var tasksCountAfter = JsonSerializer.Deserialize<List<Task>>(responseAllAfter.Content).Count;

            Assert.That(tasksCountAfter < tasksCountBefore);
        }

        [Test]
        public void Test_DeleteTask_InvalidId()
        {
            long invalidId = DateTime.Now.Ticks;
            string errMsg = "{\"errMsg\":\"Cannot find task by id: " + invalidId + "\"}";

            //Delete Task
            RestRequest requestDelete = new RestRequest(baseURl + "/api/tasks/" + invalidId, Method.Delete);

            RestResponse responseDelete = this.client.Execute(requestDelete);

            Assert.AreEqual(HttpStatusCode.NotFound, responseDelete.StatusCode);

            Assert.AreEqual(errMsg, responseDelete.Content);
        }

        [Test]
        public void Test_EditTasks_ValidIndex()
        {
            string newTitle = "New Title " + DateTime.Now.Ticks;
            string newDescription = "New Description " + DateTime.Now.Ticks;
            string board = "Open";

            //Create new task
            RestRequest request = new RestRequest(baseURl + "/api/tasks", Method.Post);
            request.AddJsonBody(new { title = newTitle, description = newDescription, board = board });

            RestResponse response = this.client.Execute(request);

            string json = response.Content.Substring(28);
            json = json.Remove(json.Length - 1, 1);
            Task? createdTask = JsonSerializer.Deserialize<Task>(json);

            //Edit Created Task
            string editedTitle = "Modified Title";
            string editedDescription = "Modified Description.";

            RestRequest editRequest = new RestRequest(baseURl + "/api/tasks/" + createdTask.id, Method.Patch);
            editRequest.AddJsonBody(new {title = editedTitle, description = editedDescription});

            RestResponse editResponse = this.client.Execute(editRequest);

            Assert.AreEqual(HttpStatusCode.Created, editResponse.StatusCode);

            //Get Edited Task
            RestRequest getRequest = new RestRequest(baseURl + "/api/tasks/" + createdTask.id);

            RestResponse getResponse = this.client.Execute(getRequest);

            Task? editedTask = JsonSerializer.Deserialize<Task>(getResponse.Content);

            //Assert
            Assert.AreEqual(editedTitle, editedTask.title);
            Assert.AreEqual(editedDescription, editedTask.description);
        }

        [Test]
        public void Test_EditTasks_invalidIndex()
        {
            //Edit Task
            long invalidId = DateTime.Now.Ticks;

            string errMsg = "{\"errMsg\":\"Cannot find task by id: " + invalidId + "\"}";

            RestRequest editRequest = new RestRequest(baseURl + "/api/tasks/" + invalidId, Method.Patch);

            RestResponse editResponse = this.client.Execute(editRequest);

            Assert.AreEqual(HttpStatusCode.BadRequest, editResponse.StatusCode);

            Assert.AreEqual(errMsg, editResponse.Content);
        }
    }
}