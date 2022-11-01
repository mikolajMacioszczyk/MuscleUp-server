package groups.workoutGroup.workout;

import groups.common.innerCommunicators.AbstractHttpValidator;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@Service
public class WorkoutHttpValidator extends AbstractHttpValidator implements WorkoutValidator {

    // TODO IMPLEMENT AFTER WORKOUT SERVICE
    private static final String GET_WORKOUT_BY_ID_PATH = "";


    @Override
    public HttpStatus checkWorkoutId(UUID workoutId) {

        return OK;
    }
}
