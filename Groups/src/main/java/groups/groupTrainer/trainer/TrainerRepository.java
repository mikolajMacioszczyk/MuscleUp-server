package groups.groupTrainer.trainer;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface TrainerRepository {

    HttpStatus checkTrainerId(UUID trainerId);

    Trainer getTrainerById(UUID trainerId);
}
