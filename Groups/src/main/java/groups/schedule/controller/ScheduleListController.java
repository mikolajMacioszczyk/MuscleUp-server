package groups.schedule.controller;

import groups.common.abstracts.AbstractListController;
import groups.schedule.dto.ScheduleCell;
import groups.schedule.service.ScheduleListService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("schedule")
public class ScheduleListController extends AbstractListController {

    private final ScheduleListService scheduleListService;


    @Autowired
    private ScheduleListController(ScheduleListService scheduleListService) {

        Assert.notNull(scheduleListService, "scheduleService must not be null");

        this.scheduleListService = scheduleListService;
    }


    @GetMapping("/{id}")
    protected ResponseEntity<?> getScheduleCellById(@PathVariable("id") UUID id) {

        ScheduleCell scheduleCell = scheduleListService.composeCell(id);

        return response(OK, scheduleCell);
    }

    @GetMapping("/all")
    protected ResponseEntity<?> getScheduleCells() {

        List<ScheduleCell> scheduleCells = scheduleListService.composeAllCells();

        return response(OK, scheduleCells);
    }
}