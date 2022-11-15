package content.workout.entity;

import content.bodyPart.entity.BodyPartNameDto;
import content.common.user.User;
import content.workoutExercise.entity.WorkoutExerciseDto;

import java.util.List;
import java.util.UUID;

public record WorkoutDto(
        UUID id,
        User creator,
        String workoutDescription,
        String workoutName,
        List<WorkoutExerciseDto> exercises,
        List<BodyPartNameDto> bodyParts
) { }