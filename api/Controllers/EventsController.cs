﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Models.DTOs;
using api.Services;
using api.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Page and PageSize must be greater than zero.");
                _logger.LogInformation($"Fetching products from page {page} with page size {pageSize}.");

                _logger.LogInformation("Fetching all events from the database.");
                var events = await _eventService.GetAllEventsAsync(page, pageSize);
                if (events == null || !events.Any())
                {
                    _logger.LogWarning("No events found in the database.");
                    return NotFound("No events found.");
                }
                var eventDtos = events.Adapt<IEnumerable<EventDTO>>();
                _logger.LogInformation($"Found {events.Count()} events.");
                return Ok(eventDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching events.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching events.");
            }
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching event with ID {id} from the database.");
                var @event = await _eventService.GetEventByIdAsync(id);
                if (@event == null)
                {
                    _logger.LogWarning($"Event with ID {id} not found.");
                    return NotFound($"Event with ID {id} not found.");
                }
                _logger.LogInformation($"Event with ID {id} found.");
                var eventDto = @event.Adapt<EventDTO>();
                return Ok(eventDto);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while fetching event with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching event with ID {id}.");
            }
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventDTO eventDto)
        {
            try
            {
                if (id != eventDto.Id)
                {
                    _logger.LogWarning($"Event ID mismatch: expected {id}, received {eventDto.Id}.");
                    return BadRequest("Event ID mismatch.");
                }

                _logger.LogInformation($"Updating event with ID {id}.");
                var @event = eventDto.Adapt<Event>();
                await _eventService.UpdateEventAsync(id, @event);
                _logger.LogInformation($"Event with ID {id} updated successfully.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _eventService.GetEventByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Event with ID {id} not found for update.");
                    return NotFound($"Event with ID {id} not found.");
                }
                else
                {
                    _logger.LogError($"Concurrency error occurred while updating event with ID {id}.");
                    throw;
                }
            }
            catch (Exception exception)     
            {
                _logger.LogError(exception, $"An error occurred while updating event with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating event with ID {id}.");
            }
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventDTO>> PostEvent(CreateEventDTO eventDto)
        {
            try
            {
                if(eventDto == null)
                {
                    _logger.LogWarning("Received null event object.");
                    return BadRequest("Event cannot be null.");
                }
                var @event = eventDto.Adapt<Event>();
                _logger.LogInformation("Adding a new event to the database.");
                await _eventService.AddEventAsync(@event);
                _logger.LogInformation($"Event with ID {@event.Id} added successfully.");
                var createdDto = @event.Adapt<EventDTO>();
                return CreatedAtAction("GetEvent", new { id = @event.Id }, createdDto);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while adding a new event.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the event.");
            }
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var @event = await _eventService.GetEventByIdAsync(id);
                if (@event == null)
                {
                    _logger.LogWarning($"Event with ID {id} not found for deletion.");
                    return NotFound($"Event with ID {id} not found.");
                }
                await _eventService.DeleteEventAsync(id);
                _logger.LogInformation($"Event with ID {id} deleted successfully.");
                return NoContent();

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while deleting event with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting event with ID {id}.");
            }
        }

        // GET: api/Events/dateRange
        [HttpGet("dateRange")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Fetching events between {startDate} and {endDate}.");
                var events = await _eventService.GetEventsByDateRangeAsync(startDate, endDate);
                if (events == null || !events.Any())
                {
                    _logger.LogWarning($"No events found between {startDate} and {endDate}.");
                    return NotFound($"No events found between {startDate} and {endDate}.");
                }
                var eventDtos = events.Adapt<IEnumerable<EventDTO>>();
                _logger.LogInformation($"Found {events.Count()} events between {startDate} and {endDate}.");
                return Ok(eventDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while fetching events between {startDate} and {endDate}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching events between {startDate} and {endDate}.");
            }
        }

    }
}
