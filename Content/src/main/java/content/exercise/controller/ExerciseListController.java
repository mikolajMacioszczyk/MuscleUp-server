package content.exercise.controller;

import content.common.abstracts.AbstractListController;
import content.exercise.entity.ExerciseDto;
import content.exercise.repository.ExerciseQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

import static org.springframework.http.HttpStatus.NOT_FOUND;
import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("exercise")
public class ExerciseListController extends AbstractListController {

    private final ExerciseQuery exerciseQuery;


    @Autowired
    private ExerciseListController(ExerciseQuery exerciseQuery) {

        Assert.notNull(exerciseQuery, "exerciseQuery must not be null");

        this.exerciseQuery = exerciseQuery;
    }


    @GetMapping("/{id}")
    protected ResponseEntity<?> getExerciseById(@RequestHeader(FITNESS_CLUB_HEADER) UUID fitnessClubId,
                                                @PathVariable("id") UUID id) {

        Optional<ExerciseDto> exerciseDto = exerciseQuery.findById(id, fitnessClubId);

        return exerciseDto.isPresent() ? response(OK, exerciseDto.get()) : response(NOT_FOUND);
    }

    @GetMapping("/all")
    protected ResponseEntity<?> getAllExercises(@RequestHeader(FITNESS_CLUB_HEADER) UUID fitnessClubId) {

        List<ExerciseDto> exercises = exerciseQuery.getAllExercises(fitnessClubId);

        return response(OK, exercises);
    }

    @GetMapping("/all-active")
    protected ResponseEntity<?> getAllActiveExercises(@RequestHeader(FITNESS_CLUB_HEADER) UUID fitnessClubId) {

        List<ExerciseDto> exercises = exerciseQuery.getAllActiveExercises(fitnessClubId);

        return response(OK, exercises);
    }
}