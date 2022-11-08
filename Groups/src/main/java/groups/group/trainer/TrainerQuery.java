package groups.group.trainer;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface TrainerQuery {

    HttpStatus checkTrainerId(UUID trainerId);

    Trainer getTrainerById(UUID trainerId);
}
